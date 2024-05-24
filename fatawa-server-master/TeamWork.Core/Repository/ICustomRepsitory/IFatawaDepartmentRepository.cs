using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.Core.Repository.ICustomRepsitory
{
    public interface IFatawaDepartmentRepository : IRepository<FatawaDepartment>
    {
        Task<List<FatawaDepartmentModel>> GetDepartmentsByLanguageId();


        Task<FatawaDepartmentModel> GetFatawaDepartmentBylanguage(int departmentId, int languageId);
    }
}
