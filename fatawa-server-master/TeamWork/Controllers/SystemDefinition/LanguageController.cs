using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.SystemDefinition
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public LanguageController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }
        [HttpGet]
        [Route("GetAllLanguages")]
        public async Task<IActionResult> GetAllLanguages()
        {
            try
            {
                List<LanguageModel> languageModel = await _unitOfWorkService.LanguageService.GetAllLanguagesAsync();
                return Ok(languageModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("GetLanguages")]
        public async Task<IActionResult> GetLanguages(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                return Ok(await _unitOfWorkService.LanguageService.GetAllLanguagesAsync(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetLanguageById")]
        public IActionResult GetLanguageById(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.LanguageService.GetLanguageById(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AddLanguage")]
        public IActionResult AddLanguage([FromBody] LanguageModel languageModel)
        {
            try
            {
                return Ok(_unitOfWorkService.LanguageService.AddLanguage(languageModel));
            }
            catch (Exception)
            {
                return BadRequest("false");
            }
        }

        [HttpPut]
        [Route("UpdateLanguage")]
        public async Task<IActionResult> UpdateLanguage(LanguageModel languageModel)
        {
            try
            {
                if (languageModel.Id != 0)
                    await _unitOfWorkService.LanguageService.UpdateLanguageAsync(languageModel);
            }
            catch (Exception)
            {
                return BadRequest("false");
            }

            return Ok("true");
        }

        [HttpDelete]
        [Route("DeleteLanguage")]
        public IActionResult DeleteLanguage(int id)
        {
            try
            {
                _unitOfWorkService.LanguageService.DeleteLanguage(id);
            }
            catch (Exception)
            {
                BadRequest("false");
            }

            return Ok("true");
        }
    }
}
