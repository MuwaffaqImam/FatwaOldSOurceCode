using System.Collections.Generic;
using TeamWork.Core.Models.Membership;

namespace TeamWork.Core.IServices.Membership
{
    public interface IMemberService
    {
        public List<MemberModel> GetAllMembers();
        public MemberModel GetMember(int id);
        public int AddMember(MemberModel model);
        public bool UpdateMember(MemberModel model);
        public bool DeleteMember(int id);
    }
}
