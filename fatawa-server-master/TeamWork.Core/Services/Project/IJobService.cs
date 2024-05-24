using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Project;
using TeamWork.Core.Services;
using TeamWork.Core.Sheard;

namespace TeamWork.Core.IServices.Project
{
    public interface IJobService : ICommonService<JobModel>
    {
        Task<List<JobModel>> GetAllAsync();
        Task<PaginationRecord<JobModel>> GetAllAsync(int pageIndex, int pageSize);
    }
}
