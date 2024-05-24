using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.IServices.SystemDefinition;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Sheard;

namespace TeamWork.Controllers.SystemDefinition
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : TeamControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PaginationRecord<TagModel> paginationRecord = new PaginationRecord<TagModel>
                {
                    DataRecord = _tagService.GetAllTags(),
                    CountRecord = _tagService.GetCountRecord(),
                };

                return Ok(paginationRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetTag")]
        public IActionResult GetTag(int id)
        {
            try
            {
                TagModel tag = _tagService.GetTag(id);
                return Ok(tag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TagModel model)
        {
            try
            {
                if (model.Id > 0)
                {
                    bool isUpdated = _tagService.UpdateTage(model);
                    return Ok(isUpdated);
                }
                bool isAdded = _tagService.AddTag(model);
                return Ok(isAdded);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool isDelete = _tagService.DeleteTag(id);
                return Ok(isDelete);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
