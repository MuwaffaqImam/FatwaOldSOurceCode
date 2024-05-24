using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.DragDrop;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.IService.Fatawa
{
    public interface IFatawaDepartmentService
    {
        Task<List<FatawaDepartmentModel>> GetDepartmentsByLanguageId();
        Task<FatawaDepartmentModel> GetFatawaDepartment(int id);
        Task<FatawaDepartmentModel> GetFatawaDepartmentBylanguage(int departmentId, int languageId);
        int AddFatawaDepartment(FatawaDepartmentModel model);
        bool UpdateFatawaDepartment(FatawaDepartmentModel model);
        bool DeleteFatawaDepartment(int id);
        Task<List<FatawaDepartmentModel>> GetDepartmentsByLevelId(int id);
        Task<List<FatawaDepartmentModel>> GetDepartmentsByDepartmentIdLevelId(int depId, int id);
        bool UpdateDropDepartment(DragDropModel dragDropModel);
    }
}
