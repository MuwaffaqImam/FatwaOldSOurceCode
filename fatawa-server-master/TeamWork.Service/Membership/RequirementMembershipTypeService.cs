using System;
using System.Linq;
using TeamWork.Core.Entity.Membership;
using TeamWork.Core.IServices.Membership;
using TeamWork.Core.Repository;

namespace TeamWork.Service.Membership
{
    public class RequirementMembershipTypeService : IRequirementMembershipTypeService
    {
        private readonly IRepository<RequirementMembershipType> _requirementMembershipType;

        public RequirementMembershipTypeService(IRepository<RequirementMembershipType> requirementMembershipType)
        {
            _requirementMembershipType = requirementMembershipType;
        }

        public bool AssignRequirementMembershipType(int requirementId, int membershipTypeId)
        {
            try
            {
                return _requirementMembershipType.Insert(new RequirementMembershipType()
                {
                    RequirementId = requirementId,
                    MembershipTypeId = membershipTypeId,
                }) == 0 ? false : true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveRequirementMembershipType(int requirementId, int membershipTypeId)
        {
            try
            {
                RequirementMembershipType requirementMembershipType =
                    _requirementMembershipType
                    .GetAll()
                    .FirstOrDefault(s => s.RequirementId == requirementId && s.MembershipTypeId == membershipTypeId);

                return _requirementMembershipType.Delete(requirementMembershipType);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveRequirementMembershipType(int id)
        {
            try
            {
                RequirementMembershipType requirementMembershipType =
                    _requirementMembershipType
                    .GetAll()
                    .FirstOrDefault(s => s.Id == id);

                return _requirementMembershipType.Delete(requirementMembershipType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
