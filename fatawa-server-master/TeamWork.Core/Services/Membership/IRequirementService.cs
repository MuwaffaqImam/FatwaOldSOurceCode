using System.Collections.Generic;
using TeamWork.Core.Models.Membership;

namespace TeamWork.Core.IServices.Membership
{
    public interface IRequirementService
    {
        List<RequirementModel> GetAllRequirements();
        RequirementModel GetRequirement(int id);
        int AddRequirement(RequirementModel model);
        bool UpdateRequirement(RequirementModel model);
        bool DeleteRequirement(int id);
    }
}
