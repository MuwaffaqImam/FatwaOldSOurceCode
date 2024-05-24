using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Services;
using TeamWork.Core.Sheard;

namespace TeamWork.Controllers.Security
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public UserController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<UserModel> userModels = await _unitOfWorkService.SecurityService.GetAllUsersAsync();
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getAllMufti")]
        public async Task<IActionResult> GetAllMufti()
        {
            try
            {
                List<UsertreeModel> fatawaDepartmentModels = await _unitOfWorkService.SecurityService.GetAllMufti();
                return Ok(fatawaDepartmentModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getAllMuftiByLanguageId")]
        public async Task<IActionResult> getAllMuftiByLanguageId(int langId)
        {
            try
            {
                List<UsertreeModel> fatawaDepartmentModels = await _unitOfWorkService.SecurityService.getAllMuftiByLanguageId(langId);
                return Ok(fatawaDepartmentModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getMuftiListByUserId")]
        public async Task<IActionResult> getMuftiListByUserId(int userId)
        {
            try
            {
                List<UsertreeModel> fatawaDepartmentModels = await _unitOfWorkService.SecurityService.getMuftiListByUserId(userId);
                return Ok(fatawaDepartmentModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("AddUserMufti")]
        public IActionResult AddUserMufti(UsertreeModel usertreeModel)
        {
            try
            {
                return Ok(_unitOfWorkService.SecurityService.AddUserMufti(usertreeModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetAllSuperAdmins")]
        public async Task<IActionResult> GetAllSuperAdminsAsync()
        {
            try
            {
                List<UserModel> userModels = await _unitOfWorkService.SecurityService.GetAllSuperAdminsAsync();
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllAdmins")]
        public async Task<IActionResult> GetAllAdminsAsync()
        {
            try
            {
                List<UserModel> userModels = await _unitOfWorkService.SecurityService.GetAllAdminsAsync();
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getAllResearchHelper")]
        public async Task<IActionResult> getAllResearchHelperAsync()
        {
            try
            {
                List<UserModel> userModels = await _unitOfWorkService.SecurityService.getAllResearchHelperAsync();
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PaginationRecord<UserModel> paginationRecord = new PaginationRecord<UserModel>
                {
                    DataRecord = await _unitOfWorkService.SecurityService.GetAllUsersAsync(pageIndex, pageSize),
                    CountRecord = _unitOfWorkService.SecurityService.GetUsersCountRecord(),
                };

                return Ok(paginationRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFilteredUsers")]
        public async Task<IActionResult> GetFilteredUsersAsync( string searchText = "", int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var list = await _unitOfWorkService.SecurityService.GetFilteredUsersAsync(searchText, pageIndex, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] UserModel userModel)
        {

            return Ok(userModel.UserId);
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] UserModel userModel)
        {
            return Ok("true");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                return Ok(await _unitOfWorkService.SecurityService.Delete(userId));
            }
            catch (Exception)
            {
                return BadRequest("false");
            }
        }

        [HttpPut]
        [Route("UpdateUserLanguage")]
        public async Task<IActionResult> UpdateUserLanguage(int languageId)
        {
            try
            {
                if (languageId != 0)
                    await _unitOfWorkService.SecurityService.UpdateUserLanguage(CurrentUserId, languageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok("true");
        }

        [HttpPut]
        [Route("TransferQuestionToMufti")]
        public async Task<IActionResult> TransferQuestionToMufti(int userId,int muftiId, int questionId)
        {
            try
            {
                if (userId != 0 && muftiId !=0)
                    await _unitOfWorkService.SecurityService.TransferQuestionToMufti(CurrentUserId, muftiId,questionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok("true");
        }



        [HttpGet]
        [Route("GetLanguageInformations")]
        public async Task<IActionResult> GetLanguageInformations()
        {
            try
            {
                LanguageModel languageModel = await _unitOfWorkService.SecurityService.GetLanguageInformations(CurrentUserId);
                return Ok(languageModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                List<Role> roles = await _unitOfWorkService.SecurityService.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UserModel userModel)
        {
            try
            {
                return Ok(await _unitOfWorkService.SecurityService.UpdateUserRole(userModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteMuftiTree")]
        public IActionResult DeleteMuftiTree(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.SecurityService.DeleteMuftiTree(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
