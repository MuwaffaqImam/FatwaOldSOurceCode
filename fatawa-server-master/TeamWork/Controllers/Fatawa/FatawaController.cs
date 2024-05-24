using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Services;
using TeamWork.Core.Sheard;

namespace TeamWork.Controllers.Fatawa
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FatawaController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public FatawaController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("GetFatawa")]
        public async Task<IActionResult> GetFatawa(int id)
        {
            try
            {
                FatawaModel fatawaModel = await _unitOfWorkService.FatawaService.GetFatawaAsync(id);
                return Ok(fatawaModel);
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFatawaDefaultSettings")]
        public IActionResult GetFatawaDefaultSettings(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaService.GetFatawaDefaultSettings(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFatawaDefaultSettingsByUser")]
        public async Task<IActionResult> GetFatawaDefaultSettingsByUserAsync()
        {
            try
            {
                FatawaDefaultSettingModel fatawaDefaultSettingModel = await _unitOfWorkService.FatawaService.GetFatawaDefaultSettingsByUserAsync();
                return Ok(fatawaDefaultSettingModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("SaveFatawa")]
        public IActionResult SaveFatawa(FatawaModel model)
        {
            try
            {
                int recordId = model.Id > 0 ? _unitOfWorkService.FatawaService.UpdateFatawa(model) : _unitOfWorkService.FatawaService.AddFatawa(model, CurrentUserId);
                return Ok(recordId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("updateImportedFatawa")]
        public IActionResult updateImportedFatawa(FatawaImportModel model)
        {
            try
            {
                _unitOfWorkService.FatawaService.updateImportedFatawa(model);
                return Ok(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("SaveFatawaDefaultSettings")]
        public IActionResult SaveFatawaDefaultSettings(FatawaDefaultSettingModel model)
        {
            try
            {
                int recordId = model.Id > 0 ? _unitOfWorkService.FatawaService.UpdateFatawaDefaultSettings(model) : _unitOfWorkService.FatawaService.AddFatawaDefaultSettings(model);
                return Ok(recordId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetFatawasLive")]
        public async Task<IActionResult> GetFatawaLive(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PaginationRecord<FatawaModel> paginationRecord = new PaginationRecord<FatawaModel>
                {
                    DataRecord = await _unitOfWorkService.FatawaService.GetAllFatawaLiveAsync(pageIndex, pageSize, CurrentUserId),
                    CountRecord = await _unitOfWorkService.FatawaService.GetAllFatawaLiveCount(),
                };

                return Ok(paginationRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFatawasArchived")]
        public async Task<IActionResult> GetFatawaArchived(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PaginationRecord<FatawaModel> paginationRecord = new PaginationRecord<FatawaModel>
                {
                    DataRecord = await _unitOfWorkService.FatawaService.GetAllFatawaArchivedAsync(pageIndex, pageSize),
                    CountRecord = await _unitOfWorkService.FatawaService.GetAllFatawaArchivedCount(),
                };

                return Ok(paginationRecord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFatawaLiveAndArchivedFiltered")]
        public async Task<IActionResult> GetFatawaLiveAndArchivedFilteredAsync(int departmentId = 0, string searchText = "", int typeId = 0, int pageIndex = 1, int pageSize = 10,bool SameTitle = false)
        {
            try
            {
                var list = await _unitOfWorkService.FatawaService.GetFatawaLiveAndArchivedFilteredAsync(departmentId, searchText, typeId, pageIndex, pageSize,SameTitle);
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getFatawaFilteredWithMathabandMufti")]
        public async Task<IActionResult> getFatawaFilteredWithMathabandMuftiAsync(int departmentId = 0, int typeId=0,int mathhabId = 0,int muftiId= 0, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var list = await _unitOfWorkService.FatawaService.getFatawaFilteredWithMathabandMufti(departmentId, typeId, mathhabId, muftiId, pageIndex, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetAllFatawasDefaultSettings")]
        public async Task<IActionResult> GetAllFatawasDefaultSettings(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                PaginationRecord<FatawaDefaultSettingModel> paginationRecord = new PaginationRecord<FatawaDefaultSettingModel>
                {
                    DataRecord = await _unitOfWorkService.FatawaService.GetAllFatawasDefaultSettings(),
                    CountRecord = await _unitOfWorkService.FatawaService.GetAllFatawasDefaultSettingsCount(),
                };

                return Ok(paginationRecord);
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
                throw ex;
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
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetFatawaStatuses")]
        public async Task<IActionResult> GetFatawaStatuses()
        {
            try
            {
                IEnumerable<StatusModel> list = await _unitOfWorkService.FatawaService.GetFatawaStatusAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetTranslators")]
        public async Task<IActionResult> GetTranslators()
        {
            try
            {
                IEnumerable<string> list = await _unitOfWorkService.FatawaService.GetTranslators();
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaService.DeleteFatawa(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteFatawaDefaultSettings")]
        public IActionResult DeleteFatawaDefaultSettings(int id)
        {
            try
            {
                return Ok(_unitOfWorkService.FatawaService.DeleteFatawaDefaultSettings(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
