using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Membership
{
    public class RequirementMembershipType : BaseEntity
    {
        public int RequirementId { get; set; }
        [ForeignKey("RequirementId")]
        public virtual Requirement Requirement { get; set; }

        public int MembershipTypeId { get; set; }
        [ForeignKey("MembershipTypeId")]
        public virtual MembershipType MembershipType { get; set; }
    }
}
