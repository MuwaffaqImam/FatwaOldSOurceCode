using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.SystemDefinition;

namespace TeamWork.Core.IServices.Project
{
    public interface IGeneralSettingsService
    {
        Task<List<GeneralSettingModel>> GetAllAsync();
        Task<bool> UpdateGeneralSettingsAsync(List<GeneralSettingModel> generalSettings);
    }
}
