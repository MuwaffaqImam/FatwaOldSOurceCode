using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TeamWork.Core.Entity;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.Notification;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Entity.SystemDefinition;

namespace TeamWork.Repository.DataContext
{
    public class TeamDataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                     IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Job> Job { get; set; }
        public DbSet<GeneralSetting> GeneralSetting { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionDiscussion> QuestionDiscussion { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TagTranslation> TagTranslations { get; set; }
        public DbSet<Fatawa> Fatawa { get; set; }
        public DbSet<FatawaDefaultSetting> FatawaDefaultSetting { get; set; }
        public DbSet<FatawaType> FatawaTypes { get; set; }
        public DbSet<FatawaTypeTranslation> FatawaTypeTranslations { get; set; }
        public DbSet<FatawaDepartment> FatawaDepartments { get; set; }
        public DbSet<FatawaDepartmentTranslation> FatawaDepartmentTranslations { get; set; }
        public DbSet<UserTree> UserTree { get; set; }
        public DbSet<FatawaTranslation> FatawaTranslation { get; set; }
        public DbSet<FatawaDefaultSettingTranslation> FatawaDefaultSettingTranslation { get; set; }
        public DbSet<FatawaStatus> FatawaStatus { get; set; }
        public DbSet<FatawaStatusTranslation> FatawaStatusTranslations { get; set; }
        public DbSet<FatawaMathhab> FatawaMathhab { get; set; }
        public DbSet<FatawaMathhabTranslation> FatawaMathhabTranslation { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<LanguageTl> LanguageTl { get; set; }
        public DbSet<NotificationItem> NotificationItem { get; set; }
        public DbSet<NotificationItemTranslation> NotificationItemTranslation { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplate { get; set; }
        public DbSet<NotificationTemplateTranslation> NotificationTemplateTranslation { get; set; }

        public TeamDataContext(DbContextOptions<TeamDataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public int UserId => int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        const int Default_User_Language_ID = 1;

        public async Task<int> GetUserLanguageId()
        {
            try
            {
                StringValues languages = new StringValues();

                if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Accept-Language", out languages))
                {
                    return Default_User_Language_ID;
                }
                return StringValues.IsNullOrEmpty(languages) || languages.Count == 0 ? Default_User_Language_ID : int.Parse(languages[0]);
            }
            catch { return Default_User_Language_ID; }
        }

        public override int SaveChanges()
        {
            return this.SaveChangesAsync(true).Result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = UserId;
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = UserId;
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(
                userRole =>
                {
                    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });
                    userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(fur => fur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                    userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(fur => fur.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
                }
            );

            builder.Entity<GeneralSetting>().ToTable("GeneralSettings").HasData(new List<GeneralSetting>
            {
                new GeneralSetting
                {
                    Id = 1,
                    SettingName = "fatawa_default_image",
                    SettingValue = ""
                }
            });

            builder.Entity<Role>().ToTable("Roles").HasData(new List<Role>
            {
                new Role {
                    Id = 1,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    PowerLevel = 99
                },
                new Role {
                    Id = 2,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    PowerLevel = 98
                },
                new Role {
                    Id = 3,
                    Name = "AdminGroup",
                    NormalizedName = "ADMINGROUP",
                    PowerLevel = 97
                },
                new Role {
                    Id = 4,
                    Name = "Student",
                    NormalizedName = "STUDENT",
                    PowerLevel = 96
                },
                new Role {
                    Id = 5,
                    Name = "Guest",
                    NormalizedName = "GUEST",
                    PowerLevel = 95
                },
                 new Role {
                    Id = 6,
                    Name = "UsersAdmin",
                    NormalizedName = "USERSADMIN",
                    PowerLevel = 100
                },
                 new Role {
                    Id = 7,
                    Name = "StudentAdmin",
                    NormalizedName = "STUDENTADMIN",
                    PowerLevel = 101
                },
            });

            builder.Entity<Language>().ToTable("Language").HasData(new List<Language>
            {
                new Language {
                    Id = 1,
                    LanguageCode="AR",
                    LanguageDefaultDisply = "العربية",
                    LanguageDirection = "RTL",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 2,
                    LanguageCode="EN",
                    LanguageDefaultDisply = "English",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 3,
                    LanguageCode="KK",
                    LanguageDefaultDisply = "қазақ тілі",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 4,
                    LanguageCode="KY",
                    LanguageDefaultDisply = "Кыргызча",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 5,
                    LanguageCode="RU",
                    LanguageDefaultDisply = "русский",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 6,
                    LanguageCode="UZ",
                    LanguageDefaultDisply = "Ўзбек",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 7,
                    LanguageCode="ZH",
                    LanguageDefaultDisply = "中文",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 8,
                    LanguageCode="UR",
                    LanguageDefaultDisply = "اُردُو‎",
                    LanguageDirection = "RTL",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
                new Language {
                    Id = 9,
                    LanguageCode="DE",
                    LanguageDefaultDisply = "Deutsch",
                    LanguageDirection = "LTR",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    LanguageFlag = "",
                },
            }); ;

            builder.Entity<LanguageTl>().ToTable("LanguageTl").HasData(new List<LanguageTl>
            {
                new LanguageTl {
                    Id = 1,
                    LanguageId = 1,
                    LanguageCode= "AR",
                    Name = "Arabic",
                },
                new LanguageTl {
                    Id = 2,
                    LanguageId = 2,
                    LanguageCode= "AR",
                    Name = "Arabic",
                },
                new LanguageTl {
                    Id = 3,
                    LanguageId = 1,
                    LanguageCode= "EN",
                    Name = "English",
                },
                new LanguageTl {
                    Id = 4,
                    LanguageId = 2,
                    LanguageCode= "EN",
                    Name = "English",
                },
                new LanguageTl {
                    Id = 5,
                    LanguageId = 1,
                    LanguageCode= "KK",
                    Name = "Kazakh",
                },
                new LanguageTl {
                    Id = 6,
                    LanguageId = 2,
                    LanguageCode= "KK",
                    Name = "Kazakh",
                },
                new LanguageTl {
                    Id = 7,
                    LanguageId = 1,
                    LanguageCode= "KY",
                    Name = "Kyrgyz",
                },
                new LanguageTl {
                    Id = 8,
                    LanguageId = 2,
                    LanguageCode= "KY",
                    Name = "Kyrgyz",
                },
                new LanguageTl {
                    Id = 9,
                    LanguageId = 1,
                    LanguageCode= "RU",
                    Name = "Russian",
                },
                new LanguageTl {
                    Id = 10,
                    LanguageId = 2,
                    LanguageCode= "RU",
                    Name = "Russian",
                },
                new LanguageTl {
                    Id = 11,
                    LanguageId = 1,
                    LanguageCode= "UZ",
                    Name = "Uzbek",
                },
                new LanguageTl {
                    Id = 12,
                    LanguageId = 2,
                    LanguageCode= "UZ",
                    Name = "Uzbek",
                },
                new LanguageTl {
                    Id = 13,
                    LanguageId = 1,
                    LanguageCode= "ZH",
                    Name = "Chinese",
                },
                new LanguageTl {
                    Id = 14,
                    LanguageId = 2,
                    LanguageCode= "ZH",
                    Name = "Chinese",
                },
                new LanguageTl {
                    Id = 15,
                    LanguageId = 1,
                    LanguageCode= "UR",
                    Name = "Urdu",
                },
                new LanguageTl {
                    Id = 16,
                    LanguageId = 2,
                    LanguageCode= "UR",
                    Name = "Urdu",
                },
                new LanguageTl {
                    Id = 17,
                    LanguageId = 1,
                    LanguageCode= "DE",
                    Name = "German",
                },
                new LanguageTl {
                    Id = 18,
                    LanguageId = 2,
                    LanguageCode= "DE",
                    Name = "German",
                },
            });

            builder.Entity<NotificationType>().ToTable("NotificationType").HasData(new List<NotificationType>
            {
                new NotificationType {
                    Id = 10,
                    Name = "Fatawa",
                },
                new NotificationType {
                    Id = 20,
                    Name = "Questions",
                },
                new NotificationType
                {
                    Id=30,
                    Name="Chatting",
                }
            });

            builder.Entity<NotificationTemplate>().ToTable("NotificationTemplate").HasData(new List<NotificationTemplate>
            {
                new NotificationTemplate {
                    Id = 1,
                    NotificationTypeId = 10
                },
                new NotificationTemplate {
                    Id = 2,
                    NotificationTypeId = 20
                },
                new NotificationTemplate
                {
                    Id = 3,
                    NotificationTypeId = 30
                }
            });

            builder.Entity<NotificationTemplateTranslation>().ToTable("NotificationTemplateTranslation").HasData(new List<NotificationTemplateTranslation>
            {
                new NotificationTemplateTranslation {
                    Id = 1,
                    NotificationTypeId = 10,
                    NotificationTemplateId = 1,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "New Fatwa was added",
                },

                new NotificationTemplateTranslation {
                    Id = 2,
                    NotificationTypeId = 10,
                    NotificationTemplateId = 1,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    Name = "New Fatwa was added",
                },

                new NotificationTemplateTranslation {
                    Id = 3,
                    NotificationTypeId = 20,
                    NotificationTemplateId = 2,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "New Question was added",
                },
                new NotificationTemplateTranslation {
                    Id = 4,
                    NotificationTypeId = 20,
                    NotificationTemplateId = 2,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    Name = "New Question was added",
                },
                new NotificationTemplateTranslation
                {
                    Id = 5,
                    NotificationTypeId = 30,
                    NotificationTemplateId = 3,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "New Message was added",
                },
                new NotificationTemplateTranslation
                {
                    Id = 6,
                    NotificationTypeId = 30,
                    NotificationTemplateId = 3,
                    LanguageId= 2,
                    LanguageCode = "En",
                    Name = "New Message was added",
                },

            });

            builder.Entity<FatawaTypeTranslation>().ToTable("FatawaTypeTranslation").HasData(new List<FatawaTypeTranslation>
            {
                new FatawaTypeTranslation {
                    Id = 1,
                    FatawaTypeId = 1,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "Fatawa Archived",
                },

                new FatawaTypeTranslation {
                    Id = 2,
                    FatawaTypeId = 1,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    Name = "Fatawa Archived",
                },

                new FatawaTypeTranslation {
                    Id = 3,
                    FatawaTypeId = 2,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "Fatawa Live",
                },
                new FatawaTypeTranslation {
                    Id = 4,
                    FatawaTypeId = 2,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    Name = "Fatawa Live",
                },
            });


            builder.Entity<FatawaStatus>().ToTable("FatawaStatus").HasData(new List<FatawaStatus>
            {
                new FatawaStatus {
                    Id = 1,
                },
                new FatawaStatus {
                    Id = 2,
                },
                new FatawaStatus {
                    Id = 3,
                },
            });

            builder.Entity<FatawaType>().ToTable("FatawaType").HasData(new List<FatawaType>
            {
                new FatawaType {
                    Id = 1,
                },
                new FatawaType {
                    Id = 2,
                },
            });

            builder.Entity<FatawaStatusTranslation>().ToTable("FatawaStatusTranslation").HasData(new List<FatawaStatusTranslation>
            {
                //ToDo : Need to fix the arabic language.
                new FatawaStatusTranslation {
                    Id = 1,
                    FatawaStatusId = 1,
                    Name = "انتظار للموافقة عليها",
                    LanguageCode = "AR",
                    LanguageId = 1,
                },
                new FatawaStatusTranslation {
                    Id = 2,
                    FatawaStatusId = 1,
                    Name = "Wait For Approval",
                    LanguageCode = "EN",
                    LanguageId = 2,
                },
                //ToDo : Need to fix the arabic language.
                new FatawaStatusTranslation {
                    Id = 3,
                    FatawaStatusId = 2,
                    Name = "موافق عليه",
                    LanguageCode = "AR",
                    LanguageId = 1,
                },
                new FatawaStatusTranslation {
                    Id = 4,
                    FatawaStatusId = 2,
                    Name = "Approved",
                    LanguageCode = "EN",
                    LanguageId = 2,
                },
                //ToDo : Need to fix the arabic language.
                new FatawaStatusTranslation {
                    Id = 5,
                    FatawaStatusId = 3,
                    Name = "غير موافق عليه",
                    LanguageCode = "AR",
                    LanguageId = 1,
                },
                new FatawaStatusTranslation {
                    Id = 6,
                    FatawaStatusId = 3,
                    Name = "NotApproved",
                    LanguageCode = "EN",
                    LanguageId = 2,
                },
            });

            builder.Entity<FatawaMathhab>().ToTable("FatawaMathhab").HasData(new List<FatawaMathhab>
            {
                new FatawaMathhab {
                    Id = 1,
                },
                new FatawaMathhab {
                    Id = 2,
                },
                new FatawaMathhab {
                    Id = 3,
                },
                new FatawaMathhab {
                    Id = 4,
                },
            });

            builder.Entity<FatawaMathhabTranslation>().ToTable("FatawaMathhabTranslation").HasData(new List<FatawaMathhabTranslation>
            {
                //ToDo : Need to fix the arabic language.
                new FatawaMathhabTranslation {
                    Id= 1,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    FatawaMathhabId = 1,
                    Name = "الحنفي",
                },
                new FatawaMathhabTranslation {
                    Id= 2,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    FatawaMathhabId = 1,
                    Name = "Hanafi",
                },
                //ToDo : Need to fix the arabic language.
                new FatawaMathhabTranslation {
                    Id= 3,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    FatawaMathhabId = 2,
                    Name = "المالكي",
                },
                new FatawaMathhabTranslation {
                    Id= 4,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    FatawaMathhabId = 2,
                    Name = "Maliki",
                },
                //ToDo : Need to fix the arabic language.
                new FatawaMathhabTranslation {
                    Id= 5,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    FatawaMathhabId = 3,
                    Name = "الشافعي",
                },
                new FatawaMathhabTranslation {
                    Id= 6,
                    LanguageId= 2,
                    LanguageCode = "EN",
                    Name = "Shafii",
                    FatawaMathhabId = 3,
                },
                //ToDo : Need to fix the arabic language.
                new FatawaMathhabTranslation {
                    Id= 7,
                    LanguageId= 1,
                    LanguageCode = "AR",
                    Name = "الحنبلي",
                    FatawaMathhabId = 4,
                },
                new FatawaMathhabTranslation {
                    Id= 8,
                    LanguageId= 2,
                    Name = "Hanbali",
                    LanguageCode = "EN",
                    FatawaMathhabId = 4,
                },
            });

            //Create New Users
            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().ToTable("Users").HasData(
            new User
            {
                Id = 1, // primary key
                UserName = "أ.د صلاح محمد ابو الحاج",
                NormalizedUserName = "SUPERADMIN",
                Email = "SuperAdmin@a.com",
                NormalizedEmail = "SUPERADMIN@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 1,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 2, // primary key
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@a.com",
                NormalizedEmail = "Admin@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 2,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 3, // primary key
                UserName = "adminGroup",
                NormalizedUserName = "ADMINGROUP",
                Email = "AdminGroup@a.com",
                NormalizedEmail = "ADMINGROUP@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 1,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 4, // primary key
                UserName = "student",
                NormalizedUserName = "STUDENT",
                Email = "Student@a.com",
                NormalizedEmail = "STUDENT@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 1,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 5, // primary key
                UserName = "guest",
                NormalizedUserName = "GUEST",
                Email = "Guest@a.com",
                NormalizedEmail = "GUEST@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 1,
                SecurityStamp = Guid.NewGuid().ToString()
            },
            new User
            {
                Id = 999, // primary key
                UserName = "UsersAdmin",
                NormalizedUserName = "USERSADMIN",
                Email = "UsersAdmin@a.com",
                NormalizedEmail = "USERSADMIN@A.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "teamWork"),
                IsActive = true,
                LanguageId = 1,
                SecurityStamp = Guid.NewGuid().ToString()
            },
             new User
             {
                 Id = 99999, // primary key
                 UserName = "StudentAdmin",
                 NormalizedUserName = "STUDENTADMIN",
                 Email = "StudentAdmin@a.com",
                 NormalizedEmail = "STUDENTADMIN@A.COM",
                 EmailConfirmed = true,
                 PasswordHash = hasher.HashPassword(null, "teamWork"),
                 IsActive = true,
                 LanguageId = 1,
                 SecurityStamp = Guid.NewGuid().ToString()
             }
        );
            //Assign role to User
            builder.Entity<UserRole>().ToTable("UserRoles").HasData(
            new UserRole
            {
                RoleId = 1,
                UserId = 1
            },
            new UserRole
            {
                RoleId = 2,
                UserId = 2
            },
            new UserRole
            {
                RoleId = 3,
                UserId = 3
            },
            new UserRole
            {
                RoleId = 4,
                UserId = 4
            },
            new UserRole
            {
                RoleId = 5,
                UserId = 5
            },
            new UserRole
            {
                RoleId = 6,
                UserId = 999
            }
        );

            //Create New fatawa Department
            builder.Entity<FatawaDepartment>().ToTable("FatawaDepartment").HasData(
            new FatawaDepartment
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                NodeNumber = "1",
                NodeMain = "0",
                NodeLevelNumber = 0,
                ParentId = 1,
                Sort = 0
            }
         );

            //Create New fatawa Department Translation
            builder.Entity<FatawaDepartmentTranslation>().ToTable("FatawaDepartmentTranslation").HasData(
            new FatawaDepartmentTranslation
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                LanguageId = 1,
                LanguageCode = "AR",
                FatawaDepartmentId = 1,
                NodeName = "Fatawa Departments",
            },
            new FatawaDepartmentTranslation
            {
                Id = 2,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                LanguageId = 2,
                LanguageCode = "EN",
                FatawaDepartmentId = 1,
                NodeName = "Fatawa Departments",
            }
         );

            //Rename All Tables Identity
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        }

        public new DbSet<T> Set<T>() where T : BaseEntity
        {
            return base.Set<T>();
        }
    }
}