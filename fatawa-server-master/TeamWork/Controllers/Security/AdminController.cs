using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Services;
using TeamWork.DTO.Security;

namespace TeamWork.Controllers.Security
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public AdminController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [Authorize(Policy = "RequierUsersAdminRole")]
        [HttpGet("GetUsersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            try
            {
                return Ok(await _unitOfWorkService.SecurityService.GetUsersListAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Policy = "RequierUsersAdminRole")]
        [HttpPost]
        [Route("editRoles")]
        public async Task<IActionResult> EditRoles(string userName, RoleEdit roleEditDTO)
        {
            try
            {
                return Ok(await _unitOfWorkService.SecurityService.EditRoles(userName, roleEditDTO));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
