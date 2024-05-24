using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.Project;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.Project
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public JobsController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<JobModel> jobModels = await _unitOfWorkService.JobService.GetAllAsync();
                return Ok(jobModels);
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
                return Ok(await _unitOfWorkService.JobService.GetAllAsync(pageIndex, pageSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] JobModel jobModel)
        {
            try
            {
                return Ok(_unitOfWorkService.JobService.Insert(jobModel));
            }
            catch (Exception)
            {
                return BadRequest("false");
            }
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] JobModel jobModel)
        {
            try
            {
                if (id != 0)
                    _unitOfWorkService.JobService.Update(jobModel);
            }
            catch (Exception)
            {
                return BadRequest("false");
            }

            return Ok("true");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWorkService.JobService.Delete(id);
            }
            catch (Exception)
            {
                BadRequest("false");
            }

            return Ok("true");
        }
    }
}

