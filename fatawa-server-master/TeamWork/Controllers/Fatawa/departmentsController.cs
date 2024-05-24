using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.DragDrop;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.Fatawa
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FatawaDepartmentsController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public FatawaDepartmentsController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("GetDaprtmentsByLanguageId")]
        public async Task<IActionResult> GetDaprtmentsByLanguageId()
        {
            try
            {
                List<FatawaDepartmentModel> fatawaDepartmentModels = await _unitOfWorkService.FatawaDepartmentService.GetDepartmentsByLanguageId();
                return Ok(fatawaDepartmentModels);
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
        [Route("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                FatawaDepartmentModel fatawaDepartmentModel = await _unitOfWorkService.FatawaDepartmentService.GetFatawaDepartment(id);
                return Ok(fatawaDepartmentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AddDepartment")]
        public IActionResult AddDepartment(FatawaDepartmentModel fatawaDepartmentModel)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaDepartmentService.AddFatawaDepartment(fatawaDepartmentModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateDepartment")]
        public IActionResult UpdateDepartment(FatawaDepartmentModel fatawaDepartmentModel)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaDepartmentService.UpdateFatawaDepartment(fatawaDepartmentModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteDepartment")]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaDepartmentService.DeleteFatawaDepartment(id));
            }
            catch (Exception ex)
            {
                throw ex;
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

        [HttpPut]
        [Route("UpdateDropDepartment")]
        public IActionResult UpdateDropDepartment(DragDropModel dragDropModel)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaDepartmentService.UpdateDropDepartment(dragDropModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
