using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Sheard;

namespace TeamWork.Core.Repository.ICustomRepsitory
{
    public interface IFatawaRepository : IRepository<Fatawa>
    {
        Task<int> GetUserLanguageId();
        Task<FatawaModel> GetFatawaByLanguageAsync(int id, int languageId);
        Task<string> GetMuftiNameById(int userId);
        Task<List<FatawaModel>> GetAllFatawaLiveAsync(int pageIndex, int pageSize, int CurrentUserId);
        Task<int> GetAllFatawaArchivedCount();
        Task<List<FatawaModel>> GetAllFatawaArchivedAsync(int pageIndex, int pageSize);
        Task<int> GetAllFatawaLiveCount();
        Task<PaginationRecord<FatawaModel>> GetFatawaFilteredAsync(int departmentId, int typeId, int pageIndex, int pageSize, string searchText = "", bool justApproved = true, bool orderById = false,bool SameTitle=false);
        Task<List<FatawaTypeModel>> GetFatawaTypesAsync();
        Task<List<FatawaDepartmentModel>> GetFatawaDeparmentsAsync();
        Task<List<FatawaMathhabModel>> GetFatawaMathhabsAsync();
        Task<List<StatusModel>> GetFatawaStatusAsync();
        Task<List<string>> GetTranslators();
        Task<FatawaDefaultSettingModel> GetFatawaDefaultSettings(int id);
        Task<FatawaDefaultSettingModel> GetFatawaDefaultSettingsByUserAsync();
        Task<int> GetAllFatawasDefaultSettingsCount();
        Task<PaginationRecord<FatawaModel>> GetClientFatawaFilteredAsync(string clientSearchText, int mathhabId, int fatwaTypeId, int pageIndex, int pageSize,bool SameTitle);
        Task<List<Language>> GetFatawaLanguages(int fatwaId);
        Task<PaginationRecord<FatawaModel>> GetFatawaLiveAndArchivedFilteredAsync(int departmentId, string searchText, int typeId, int pageIndex, int pageSize,bool SameTitle);
        Task<PaginationRecord<FatawaExportModel>> getFatawaFilteredWithMathabandMufti(int departmentId, int typeId, int mathhabId, int muftiId, int pageIndex, int pageSize);

        Task<List<FatawaModel>> GetFatawaRelations(int fatwaId, int languageId);
        Task<List<TagModel>> GetFatawaTags(int fatawaId, int languageId);
    }
}
