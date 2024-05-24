using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.Fatawa
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly ILogger _logger;

        public ClientController(IUnitOfWorkService unitOfWorkService, ILogger<ClientController> logger)
        {
            _unitOfWorkService = unitOfWorkService;
            _logger = logger;
        }


        [HttpGet]
        [Route("GetFatawaFiltered")]
        public async Task<IActionResult> GetFatawaFiltered(int departmentId = 0, int typeId = 0, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var list = await _unitOfWorkService.FatawaService.GetFatawaFilteredAsync(departmentId, typeId, pageIndex, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetFatawaDeparments")]
        public async Task<IActionResult> GetFatawaDeparments()
        {
            try
            {
                IEnumerable<FatawaDepartmentModel> list = await _unitOfWorkService.FatawaService.GetFatawaDeparmentsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDepartmentsByLevelId")]
        public async Task<IActionResult> GetDepartmentsByLevelId(int levelNo)
        {
            try
            {
                List<FatawaDepartmentModel> fatawaDepartmentModel = await _unitOfWorkService.FatawaDepartmentService.GetDepartmentsByLevelId(levelNo);
                return Ok(fatawaDepartmentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetDepartmentsByDepartmentIdLevelId")]
        public async Task<IActionResult> GetDepartmentsByDepartmentIdLevelId(int departmentId, int levelNo)
        {
            try
            {
                List<FatawaDepartmentModel> fatawaDepartmentModel = await _unitOfWorkService.FatawaDepartmentService.GetDepartmentsByDepartmentIdLevelId(departmentId,levelNo);
                return Ok(fatawaDepartmentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetFatawaDepartmentBylanguage")]
        public async Task<IActionResult> GetFatawaDepartmentBylanguage(int departmentId, int languageId)
        {
            try
            {
                FatawaDepartmentModel fatawaDepartmentModels = await _unitOfWorkService.FatawaDepartmentService.GetFatawaDepartmentBylanguage(departmentId, languageId);
                return Ok(fatawaDepartmentModels);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetFatawaTypes")]
        public async Task<IActionResult> GetFatawaTypes()
        {
            try
            {
                IEnumerable<FatawaTypeModel> list = await _unitOfWorkService.FatawaService.GetFatawaTypesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetFatawaMathhabs")]
        public async Task<IActionResult> GetFatawaMathhabs()
        {
            try
            {
                IEnumerable<FatawaMathhabModel> list = await _unitOfWorkService.FatawaService.GetFatawaMathhabsAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetClientFatawaFiltered")]
        public async Task<IActionResult> GetClientFatawaFiltered(string clientSearchText, int mathhabId, int fatwaTypeId, int pageIndex, int pageSize,bool SameTitle)
        {
            try
            {
                var list = await _unitOfWorkService.FatawaService.GetClientFatawaFilteredAsync(clientSearchText, mathhabId, fatwaTypeId, pageIndex, pageSize,SameTitle);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetFatawaByLanguage")]
        public async Task<IActionResult> GetFatawaByLanguage(int id, int languageId)
        {
            try
            {
                FatawaModel fatawaModel = await _unitOfWorkService.FatawaService.GetFatawaByLanguageAsync(id, languageId);
                return Ok(fatawaModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("getFatawaRelations")]
        public async Task<IActionResult> GetFatawaRelations(int fatwaId, int languageId)
        {
            try
            {
                var list = await _unitOfWorkService.FatawaService.GetFatawaRelations(fatwaId, languageId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetFatawaLanguages")]
        public async Task<IActionResult> GetFatawaLanguages(int fatwaId)
        {
            try
            {
                var languages = await _unitOfWorkService.FatawaService.GetFatawaLanguages(fatwaId);
                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
