using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Sheard;

namespace TeamWork.IService.Fatawa
{
    public interface IFatawaService
    {
        Task<List<FatawaModel>> GetAllFatawas();
        Task<List<FatawaModel>> GetAllFatawas(string tags);
        Task<FatawaModel> GetFatawaAsync(int id);
        Task<FatawaModel> GetFatawaByLanguageAsync(int id, int languageId);
        int AddFatawa(FatawaModel model, int currentUserId);

        int UpdateFatawa(FatawaModel model);
        bool updateImportedFatawa(FatawaImportModel model);
       
        bool DeleteFatawa(int id);
        void ChangeLastSeen(int id);
        void IncreaseVistorsCount(int id);
        Task<List<FatawaModel>> GetAllFatawaLiveAsync(int pageIndex, int pageSize, int CurrentUserId);
        Task<int> GetAllFatawaArchivedCount();
        Task<List<FatawaModel>> GetAllFatawaArchivedAsync(int pageIndex, int pageSize);
        Task<int> GetAllFatawaLiveCount();
        Task<PaginationRecord<FatawaModel>> GetFatawaFilteredAsync(int departmentId, int typeId, int PageIndex, int PageSize);
        Task<List<FatawaTypeModel>> GetFatawaTypesAsync();
        Task<List<FatawaDepartmentModel>> GetFatawaDeparmentsAsync();
        Task<List<FatawaMathhabModel>> GetFatawaMathhabsAsync();
        Task<List<StatusModel>> GetFatawaStatusAsync();
        Task<List<string>> GetTranslators();
        Task<FatawaDefaultSettingModel> GetFatawaDefaultSettings(int id);
        Task<FatawaDefaultSettingModel> GetFatawaDefaultSettingsByUserAsync();
        int AddFatawaDefaultSettings(FatawaDefaultSettingModel model);
        int UpdateFatawaDefaultSettings(FatawaDefaultSettingModel model);
        Task<List<FatawaDefaultSettingModel>> GetAllFatawasDefaultSettings(int pageIndex = 1, int pageSize = 10);
        bool DeleteFatawaDefaultSettings(int id);
        Task<int> GetAllFatawasDefaultSettingsCount();

        //Changed
        Task<PaginationRecord<FatawaModel>> GetClientFatawaFilteredAsync(string clientSearchText, int mathhabId, int fatwaTypeId, int pageIndex, int pageSize,bool SameTitle);
        Task<List<Language>> GetFatawaLanguages(int fatwaId);
        Task<PaginationRecord<FatawaModel>> GetFatawaLiveAndArchivedFilteredAsync(int departmentId, string searchText, int typeId, int pageIndex, int pageSize,bool SameTitle);
        Task<PaginationRecord<FatawaExportModel>> getFatawaFilteredWithMathabandMufti(int departmentId, int typeId, int mathhabId, int muftiId, int pageIndex, int pageSize);
        Task<List<FatawaModel>> GetFatawaRelations(int fatwaId, int languageId);
    }
}
