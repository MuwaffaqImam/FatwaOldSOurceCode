using System.Collections.Generic;
using TeamWork.Core.Models.Membership;

namespace TeamWork.Core.IServices.Membership
{
    public interface IMembershipTypeService
    {
        public List<MembershipTypeModel> GetAllMembershipTypes();
        public MembershipTypeModel GetMembershipType(int id);
        public int AddMembershipType(MembershipTypeModel model);
        public bool UpdateMembershipType(MembershipTypeModel model);
        public bool DeleteMembershipType(int id);
    }
}
