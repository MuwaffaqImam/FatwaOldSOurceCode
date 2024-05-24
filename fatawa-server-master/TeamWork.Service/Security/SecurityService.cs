using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Repository;
using TeamWork.Core.Sheard;
using TeamWork.DTO.Security;

namespace TeamWork.Service.Security
{
    internal class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMapper _imapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        internal SecurityService(IUnitOfWork unitOfWork, IMapper imapper, Microsoft.Extensions.Configuration.IConfiguration iConfig)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _mapper = imapper;
                _imapper = imapper;
                _configuration = iConfig;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetUsersListAsync()
        {
            try
            {
                return await _unitOfWork.SecurityRepository.GetUsersListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetUserBySearchNameAsync(string searchUserName)
        {
            try
            {
                return await _unitOfWork.SecurityRepository.GetUserBySearchNameAsync(searchUserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return await _unitOfWork.SecurityRepository.GetAllUsersAsync(pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserModel>> GetAllSuperAdminsAsync()
        {
            try
            {
                return await GetAllUsersByTypeAsync("superadmin");
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
                return await _unitOfWork.SecurityRepository.GetAllMufti();
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
                return await _unitOfWork.SecurityRepository.getMuftiListByUserId(userId);
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
                return await _unitOfWork.SecurityRepository.getAllMuftiByLanguageId(langId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserModel>> GetAllAdminsAsync()
        {
            try
            {
                return await GetAllUsersByTypeAsync("admin");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserModel>> getAllResearchHelperAsync()
        {
            try
            {
                return await GetAllUsersByTypeAsync("StudentAdmin");
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
                return await _unitOfWork.SecurityRepository.GetAllUsersByTypeAsync(userType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUsersCountRecord()
        {
            try
            {
                return _unitOfWork.SecurityRepository.GetUsersCountRecord();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(int UserId)
        {
            try
            {
                return await _unitOfWork.SecurityRepository.Delete(UserId);
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
                return await _unitOfWork.SecurityRepository.UpdateUserLanguage(userId, languageId);
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
                return await _unitOfWork.SecurityRepository.TransferQuestionToMufti(userId, muftiId, questionId);
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
                return await _unitOfWork.SecurityRepository.GetLanguageInformations(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Role>> GetAllRoles()
        {
            try
            {
                return await _unitOfWork.SecurityRepository.GetAllRoles();
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
                int MuftiUserId = Convert.ToInt32(_configuration.GetSection("MySettings").GetSection("MuftiUserId").Value);
                if (userModel.RoleId == 1)
                {
                    AddUserMufti(new UsertreeModel
                    {
                        Id = 0,
                        ParentId = MuftiUserId,
                        mufitUserId = userModel.UserId,
                        Sort = 0,
                    });
                }

                return await _unitOfWork.SecurityRepository.UpdateUserRole(userModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteMuftiTree(int id)
        {
            try
            {
                UserTree department = _unitOfWork.GetRepository<UserTree>().GetSingle(id);
                List<UserTree> childrenfatawaDepartments = _unitOfWork.GetRepository<UserTree>().GetWhere(x => x.ParentId == department.Id).ToList();

                bool isDeleted = false;

                if (department.NodeLevelNumber == 0 || childrenfatawaDepartments.Count > 0)
                {
                    return false;
                }

                if (department != null)
                {
                    //isDeleted = DeleteTranslations(id);

                    //if (isDeleted)
                    //{
                    _unitOfWork.GetRepository<UserTree>().Delete(department);
                    isDeleted = _unitOfWork.SaveChanges();
                    //  }
                }

                return isDeleted;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public int AddUserMufti(UsertreeModel model)
        {
            try
            {
                UserTree fatawaDepartment = _unitOfWork.GetRepository<UserTree>().GetSingle(model.ParentId);
                UserTree fatawaDepartmentLastCreatedNode = _unitOfWork.GetRepository<UserTree>().GetWhere(x => x.ParentId == fatawaDepartment.Id).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                UserTree fatawaDepartmentLastSortNode = _unitOfWork.GetRepository<UserTree>().GetWhere(x => x.ParentId == fatawaDepartment.Id).OrderByDescending(o => o.Sort).FirstOrDefault();

                if (fatawaDepartmentLastCreatedNode != null && fatawaDepartmentLastCreatedNode.NodeLevelNumber != 0)
                {
                    int lengthMain = fatawaDepartmentLastCreatedNode.NodeMain.Length;
                    string siblingNodeNumber = fatawaDepartmentLastCreatedNode.NodeNumber;
                    string mainNumber = siblingNodeNumber.Substring(0, lengthMain);
                    string childNumber = siblingNodeNumber.Substring(lengthMain);

                    model.NodeNumber = mainNumber + (Convert.ToInt32(childNumber) + 1);
                    model.NodeMain = fatawaDepartmentLastCreatedNode.NodeMain;
                    model.Sort = fatawaDepartmentLastSortNode.Sort + 1;
                }
                else
                {
                    model.NodeNumber = fatawaDepartment.NodeNumber + "1";
                    model.NodeMain = fatawaDepartment.NodeNumber;
                    model.Sort = 0;
                }
                UserTree department = _imapper.Map<UserTree>(model);
                _unitOfWork.GetRepository<UserTree>().Insert(department);

                if (_unitOfWork.SaveChanges())
                {
                    model.Id = department.Id;
                    // AddTranslations(model);
                }

                return department.Id;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> SocialLogin(SocialUserModel socialUserModel)
        {
            try
            {
                HttpClient http = new HttpClient();
                string providerPrefixURL = string.Empty;

                if (socialUserModel.Provider == "FACEBOOK")
                {
                    providerPrefixURL = "https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token=";
                }
                else if (socialUserModel.Provider == "GOOGLE")
                {
                    providerPrefixURL = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=";
                }
                var httpResponse = await http.GetAsync($"{providerPrefixURL}{socialUserModel.AuthToken}");
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
                SocialUserModel userInfo = JsonConvert.DeserializeObject<SocialUserModel>(httpContent);
                var user = await _unitOfWork.SecurityRepository.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    var hasher = new PasswordHasher<User>();
                    UserRegister newUser = new UserRegister
                    {
                        UserName = userInfo.Name ?? userInfo.Email,
                        LanguageId = 1,
                        Email = userInfo.Email,
                        PhotoURL = userInfo.PhotoUrl,
                        EmailConfirmed = true,
                        IsActive = true
                    };

                    var result = await _unitOfWork.SecurityRepository.CreateUserAsync(newUser, string.Format("{0}@123", Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8)));

                    if (!result.Succeeded) throw new Exception("Failed to create local user account.");

                    await _unitOfWork.SecurityRepository.AddToRoleAsync(newUser, "guest");
                }

                // generate the jwt for the local user...
                var localUser = await _unitOfWork.SecurityRepository.FindByEmailAsync(userInfo.Email);

                if (localUser == null)
                {
                    throw new Exception("Failed to find local user account.");
                }

                var userToReturn = _mapper.Map<UserList>(localUser);
                return new
                {
                    token = _unitOfWork.SecurityRepository.GenerateJwtTokenAsync(localUser).Result,
                    user = userToReturn
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<IList<string>> EditRoles(string userName, RoleEdit roleEditDTO)
        {
            try
            {
                return _unitOfWork.SecurityRepository.EditRoles(userName, roleEditDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<object> Login(UserLogin userLogin)
        {
            try
            {
                return _unitOfWork.SecurityRepository.Login(userLogin);
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
                return _unitOfWork.SecurityRepository.FindByEmailAsync(email);
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
                return _unitOfWork.SecurityRepository.ConfirmEmailAsync(user, token);
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
                return await _unitOfWork.SecurityRepository.GenerateEmailConfirmationTokenAsync(userRegister);
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
                return _unitOfWork.SecurityRepository.CreateUserAsync(userRegister);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task AddToRoleAsync(UserRegister userRegister, string roleName)
        {
            try
            {
                return _unitOfWork.SecurityRepository.AddToRoleAsync(userRegister, roleName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> SendEmailAsync(string[] toEmails, string emailSubject, string emailBody)
        {
            try
            {
                return _unitOfWork.SecurityRepository.SendEmailAsync(toEmails, emailSubject, emailBody);
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
                return await _unitOfWork.SecurityRepository.FindByNameAsync(userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            try
            {
                return await _unitOfWork.SecurityRepository.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            try
            {
                return await _unitOfWork.SecurityRepository.GeneratePasswordResetTokenAsync(user);
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
                return _unitOfWork.SecurityRepository.GetFilteredUsersAsync(searchText, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
