namespace TeamWork.Core.IServices.Membership
{
    public interface IRequirementMembershipTypeService
    {
        bool AssignRequirementMembershipType(int requirementId, int membershipTypeId);
        bool RemoveRequirementMembershipType(int requirementId, int membershipTypeId);
        bool RemoveRequirementMembershipType(int id);
    }
}
