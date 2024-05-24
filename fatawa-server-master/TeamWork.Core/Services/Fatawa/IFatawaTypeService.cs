using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.IService.Fatawa
{
    public interface IFatawaTypeService
    {
        Task<List<FatawaTypeModel>> GetAllFatawaTypes();
        Task<FatawaTypeModel> GetFatawaType(int id);
        int AddFatawaType(FatawaTypeModel model);
        Task<bool> UpdateFatawaType(FatawaTypeModel model);
        Task<bool> DeleteFatawaType(int id);
    }
}
