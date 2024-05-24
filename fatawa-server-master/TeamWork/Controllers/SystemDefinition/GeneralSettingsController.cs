using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.SystemDefinition
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingsController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public GeneralSettingsController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGeneralSettings()
        {
            try
            {
                return Ok(await _unitOfWorkService.GeneralSettingsService.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] List<GeneralSettingModel> generalSettingModel)
        {
            try
            {
                await _unitOfWorkService.GeneralSettingsService.UpdateGeneralSettingsAsync(generalSettingModel);
            }
            catch (Exception)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }
    }
}
