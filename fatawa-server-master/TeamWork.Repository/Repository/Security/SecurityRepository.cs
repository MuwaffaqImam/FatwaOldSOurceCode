using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Core.Email;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Repository;
using TeamWork.Core.Services;
using TeamWork.Core.Sheard;
using TeamWork.DTO.Security;

namespace TeamWork.Repository.Repository.Security
{
    internal class SecurityRepository : ISecurityRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        internal SecurityRepository(IDbFactory dbFactory, IMapper mapper,
                                    UserManager<User> userManager, SignInManager<User> signInManager,
                                    IConfiguration configuration, IEmailSender emailSender)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        public async Task<List<UserModel>> GetUsersListAsync()
        {
            try
            {
                return await (from user in _dbFactory.GetDataContext.Users
                              orderby user.Id
                              select new UserModel
                              {
                                  UserId = user.Id,
                                  UserName = user.UserName,
                                  RoleName = (from userRole in user.UserRoles
                                              join role in _dbFactory.GetDataContext.Roles
                                              on userRole.RoleId equals role.Id
                                              where userRole.UserId == user.Id
                                              select role.Name).FirstOrDefault()
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<UserModel>> GetUserBySearchNameAsync(string searchUserName)
        {
            return await (from user in _dbFactory.GetDataContext.Users
                          select new UserModel
                          {
                              UserId = user.Id,
                              UserName = user.UserName,
                          }).Where(x => x.UserName == searchUserName).ToListAsync();
        }

        public async Task<List<UserModel>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return await (from user in _dbFactory.GetDataContext.Users
                              orderby user.Id
                              select new UserModel
                              {
                                  UserId = user.Id,
                                  UserName = user.UserName,
                                  Email = user.Email,
                                  FullName = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == user.Id).FullName,
                                  RoleName = (from userRole in user.UserRoles
                                              join role in _dbFactory.GetDataContext.Roles
                                              on userRole.RoleId equals role.Id
                                              where userRole.UserId == user.Id
                                              select role.Name).FirstOrDefault()
                              }).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<UsertreeModel>> GetAllMufti()
        {
            try
            {
                List<UsertreeModel> UserTreeList = await (from fd in _dbFactory.GetDataContext.UserTree
                                                          join fdt in _dbFactory.GetDataContext.Users
                                                                  on fd.mufitUserId equals fdt.Id
                                                          where fdt.LanguageId == _dbFactory.GetDataContext.GetUserLanguageId().Result 
                                                          where fdt.IsActive == true
                                                          select new UsertreeModel
                                                          {
                                                              Id = fd.Id,
                                                              CreatedBy = fd.CreatedBy,
                                                              CreatedDate = fd.CreatedDate,
                                                              UpdatedBy = fd.UpdatedBy,
                                                              UpdatedDate = fd.UpdatedDate,
                                                              NodeNumber = fd.NodeNumber,
                                                              NodeMain = fd.NodeMain,
                                                              NodeLevelNumber = fd.NodeLevelNumber,
                                                              mufitUserId = fd.mufitUserId,
                                                              NodeName = fdt.FullName,
                                                              Sort = fd.Sort

                                                          }).OrderBy(y => y.Sort).ToListAsync();

                return UserTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UsertreeModel>> getMuftiListByUserId(int userId)
        {
            try
            {
                List<UsertreeModel> UserTreeList;
                UserTree MuftiUser = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.mufitUserId == userId);
                if (MuftiUser != null)
                {
                    UserTreeList = await (from fd in _dbFactory.GetDataContext.UserTree
                                          join fdt in _dbFactory.GetDataContext.Users
                                                  on fd.mufitUserId equals fdt.Id
                                          where fd.ParentId == MuftiUser.Id && fdt.LanguageId == _dbFactory.GetDataContext.GetUserLanguageId().Result
                                          select new UsertreeModel
                                          {
                                              Id = fd.Id,
                                              CreatedBy = fd.CreatedBy,
                                              CreatedDate = fd.CreatedDate,
                                              UpdatedBy = fd.UpdatedBy,
                                              UpdatedDate = fd.UpdatedDate,
                                              NodeNumber = fd.NodeNumber,
                                              NodeMain = fd.NodeMain,
                                              NodeLevelNumber = fd.NodeLevelNumber,
                                              mufitUserId = fd.mufitUserId,
                                              NodeName = fdt.UserName,
                                              Sort = fd.Sort

                                          }).OrderBy(y => y.Sort).ToListAsync();
                }
                else
                {
                    UserTreeList = null;
                }


                return UserTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UsertreeModel>> getAllMuftiByLanguageId(int langId)
        {
            try
            {
                List<UsertreeModel> UserTreeList = await (from fd in _dbFactory.GetDataContext.UserTree
                                                          join fdt in _dbFactory.GetDataContext.Users
                                                                  on fd.mufitUserId equals fdt.Id
                                                          where fdt.LanguageId == langId
                                                          select new UsertreeModel
                                                          {
                                                              Id = fd.Id,
                                                              CreatedBy = fd.CreatedBy,
                                                              CreatedDate = fd.CreatedDate,
                                                              UpdatedBy = fd.UpdatedBy,
                                                              UpdatedDate = fd.UpdatedDate,
                                                              NodeNumber = fd.NodeNumber,
                                                              NodeMain = fd.NodeMain,
                                                              NodeLevelNumber = fd.NodeLevelNumber,
                                                              mufitUserId = fd.mufitUserId,
                                                              NodeName = fdt.FullName,
                                                              Sort = fd.Sort

                                                          }).OrderBy(y => y.Sort).ToListAsync();

                return UserTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserModel>> GetAllUsersByTypeAsync(string userType)
        {
            try
            {
                List<User> users = await _dbFactory.GetDataContext.Users.Where(user =>
                user.UserRoles.Where(ur => ur.Role.Name.ToLower() == userType).Count() > 0).ToListAsync();

                return users.Select(x => new UserModel
                {
                    UserId = x.Id,
                    UserName = x.UserName,
                    FullName = x.FullName,
                    Email = x.Email

                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetUsersCountRecord()
        {
            try
            {
                return _dbFactory.GetDataContext.Users.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                User user = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == id);

                if (user == null)
                {
                    throw new Exception("User Not Exist!");
                }

                var rolesForUser = await GetRolesAsync(user);

                if (rolesForUser.Count() > 0)
                {
                    foreach (var role in rolesForUser)
                    {
                        // item should be the name of the role
                        var result = await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                //Delete User
                await _userManager.DeleteAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUserLanguage(int userId, int languageId)
        {
            try
            {
                User user = await _dbFactory.GetDataContext.Users.FindAsync(userId);

                if (user != null)
                    user.LanguageId = languageId;

                _dbFactory.GetDataContext.Users.Update(user);
                await _dbFactory.GetDataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> TransferQuestionToMufti(int userId, int muftiId, int questionId)
        {
            try
            {
                Question question = await _dbFactory.GetDataContext.Question.FindAsync(questionId);

                if (question != null)
                {
                    question.TransferUserId = muftiId;
                    question.StatusUserId = muftiId;
                    question.StatusId = 2;
                }

                _dbFactory.GetDataContext.Question.Update(question);
                await _dbFactory.GetDataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<LanguageModel> GetLanguageInformations(int userId)
        {
            try
            {
                Language language = await (from user in _dbFactory.GetDataContext.Users
                                           join lang in _dbFactory.GetDataContext.Language on user.LanguageId equals lang.Id
                                           where user.Id == userId
                                           select new Language
                                           {
                                               Id = lang.Id,
                                               LanguageDirection = lang.LanguageDirection,

                                           }).SingleOrDefaultAsync();

                LanguageModel languageModel = _mapper.Map<LanguageModel>(language);

                return languageModel;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<List<Role>> GetAllRoles()
        {
            try
            {
                return _dbFactory.GetDataContext.Roles.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUserRole(UserModel userModel)
        {
            try
            {
                User user = await _dbFactory.GetDataContext.Users.FindAsync(userModel.UserId);
                IEnumerable<string> roles = await GetRolesAsync(user);

                if (roles != null)
                {
                    var isDeleted = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

                    if (isDeleted.Succeeded)
                    {
                        List<Role> roleList = await GetAllRoles();
                        string roleName = roleList.Find(x => x.Id == userModel.RoleId).Name;
                        await _userManager.AddToRoleAsync(user, roleName);

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<string>> EditRoles(string userName, RoleEdit roleEditDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                var userRoles = await GetRolesAsync(user);
                var selectedRoles = roleEditDTO.RoleNames;

                selectedRoles = selectedRoles ?? new string[] { }; // same ---> selectedRoles = selectedRoles != null ? selectedRoles : new string[] {};
                var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

                if (!result.Succeeded)
                    throw new Exception("Something goes wrong with adding roles!");

                result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

                if (!result.Succeeded)
                    throw new Exception("Something goes wrong with removing roles!");

                return await GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<User> FindByEmailAsync(string email)
        {
            try
            {
                return _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                var roles = await GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var tokenDedcription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDedcription);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            try
            {
                return _userManager.GeneratePasswordResetTokenAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            try
            {
                return _userManager.ResetPasswordAsync(user, token, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            try
            {
                return _userManager.ConfirmEmailAsync(user, token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Login(UserLogin userLogin)
        {
            try
            {
                var userManager = await FindByEmailAsync(userLogin.Email);

                if (userManager == null)
                {
                    return null;
                }
                var result = await _signInManager.CheckPasswordSignInAsync(userManager, userLogin.Password, false);

                if (!result.Succeeded)
                {
                    return null;
                }
                var appUser = _userManager.Users.FirstOrDefault(u => u.NormalizedEmail.ToUpper() == userLogin.Email.ToUpper());
                var userToReturn = _mapper.Map<UserList>(appUser);
                return new
                {
                    token = GenerateJwtTokenAsync(appUser).Result,
                    user = userToReturn
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IdentityResult> CreateUserAsync(UserRegister userRegister)
        {
            try
            {
                User userToCreate = _mapper.Map<User>(userRegister);
                return _userManager.CreateAsync(userToCreate, userRegister.PasswordHash);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddToRoleAsync(UserRegister userRegister, string roleName)
        {
            try
            {
                User userManager = await _userManager.FindByNameAsync(userRegister.UserName);
                await _userManager.AddToRoleAsync(userManager, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserRegister userRegister)
        {
            try
            {
                User userManager = await _userManager.FindByNameAsync(userRegister.UserName);
                return await _userManager.GenerateEmailConfirmationTokenAsync(userManager);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegister userRegister, string guid)
        {
            try
            {
                User user = _mapper.Map<User>(userRegister);
                return await _userManager.CreateAsync(user, guid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            try
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SendEmailAsync(string[] toEmails, string emailSubject, string emailBody)
        {
            try
            {
                return await _emailSender.SendEmailAsync(toEmails, emailSubject, emailBody);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            try
            {
                return await _userManager.FindByNameAsync(userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PaginationRecord<UserModel>> GetFilteredUsersAsync(string searchText, int pageIndex, int pageSize)
        {
            try
            {
                return this.GetFilteredUsers(pageIndex, pageSize, searchText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaginationRecord<UserModel>> GetFilteredUsers(int pageIndex, int pageSize, string searchText = "")
        {
            try
            {
                var fatawaQuery = _dbFactory.GetDataContext.Users.AsQueryable();

                if (!string.IsNullOrEmpty(searchText))
                {
                    fatawaQuery = fatawaQuery.Where(x => x.UserName.Contains(searchText) || x.FullName.Contains(searchText));
                }

                fatawaQuery = fatawaQuery.OrderBy(s => s.Id);

                var fatawTotalCount = fatawaQuery.Count();
                fatawaQuery = fatawaQuery.Paginate(pageIndex, pageSize);

                PaginationRecord<UserModel> paginationRecord = new PaginationRecord<UserModel>
                {
                    DataRecord = await fatawaQuery.Select(x => new UserModel
                    {

                        UserId = x.Id,
                        UserName = x.UserName,
                        FullName = x.FullName,
                        Email = x.Email,
                        RoleName = (from userRole in x.UserRoles
                                    join role in _dbFactory.GetDataContext.Roles
                                    on userRole.RoleId equals role.Id
                                    where userRole.UserId == x.Id
                                    select role.Name).FirstOrDefault()


                    }).ToListAsync(),

                    CountRecord = fatawTotalCount
                };

                return paginationRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}