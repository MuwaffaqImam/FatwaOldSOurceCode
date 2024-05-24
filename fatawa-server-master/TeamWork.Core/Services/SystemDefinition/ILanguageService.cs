using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Sheard;

namespace TeamWork.Core.IServices.SystemDefinition
{
    public interface ILanguageService
    {
        List<LanguageModel> GetAllLanguages();
        Task<List<LanguageModel>> GetAllLanguagesAsync();
        Task<PaginationRecord<LanguageModel>> GetAllLanguagesAsync(int pageIndex, int pageSize);
        int AddLanguage(LanguageModel languageModel);
        Task<bool> UpdateLanguageAsync(LanguageModel languageModel);
        LanguageModel GetLanguageById(int id);
        bool DeleteLanguage(int id);
    }
}
