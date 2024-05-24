using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamWork.Core.Email;
using TeamWork.Core.Entity.Security;

namespace TeamWork.Core.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IdentityBuilder AddDependencyInjectionCore(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.SignIn.RequireConfirmedEmail = true;
            }).AddDefaultTokenProviders();

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();
            return builder;
        }
    }
}