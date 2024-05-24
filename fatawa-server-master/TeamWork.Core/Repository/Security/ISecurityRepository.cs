using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Sheard;
using TeamWork.DTO.Security;

public interface ISecurityRepository
{
    Task<List<UserModel>> GetUsersListAsync();
    Task<List<UserModel>> GetUserBySearchNameAsync(string userName);
    Task<List<UserModel>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);
    Task<List<UserModel>> GetAllUsersByTypeAsync(string userType);
    int GetUsersCountRecord();
    Task<bool> Delete(int id);
    Task<bool> UpdateUserLanguage(int userId, int languageId);
    Task<bool> TransferQuestionToMufti(int userId, int muftiId, int questionId);
    Task<LanguageModel> GetLanguageInformations(int userId);
    Task<List<Role>> GetAllRoles();
    Task<bool> UpdateUserRole(UserModel userModel);
    Task<IList<string>> EditRoles(string userName, RoleEdit roleEditDTO);
    Task<User> FindByEmailAsync(string email);
    Task<string> GenerateJwtTokenAsync(User user);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<object> Login(UserLogin userLogin);
    Task<IdentityResult> CreateUserAsync(UserRegister userRegister);
    Task AddToRoleAsync(UserRegister userRegister, string roleName);
    Task<string> GenerateEmailConfirmationTokenAsync(UserRegister userRegister);
    Task<IdentityResult> CreateUserAsync(UserRegister userRegister, string guid);
    Task<IList<string>> GetRolesAsync(User user);
    Task<bool> SendEmailAsync(string[] toEmails, string emailSubject, string emailBody);
    Task<User> FindByNameAsync(string userName);

    Task<List<UsertreeModel>> GetAllMufti();
    Task<List<UsertreeModel>> getMuftiListByUserId(int userId);
    Task<List<UsertreeModel>> getAllMuftiByLanguageId(int langId);

    Task<PaginationRecord<UserModel>> GetFilteredUsersAsync(string searchText,int pageNumber = 1, int pageSize = 10);
}