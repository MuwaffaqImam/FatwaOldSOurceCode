using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Membership
{
    public class MembershipType : BaseEntity
    {
        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<MembershipTypeTranslation> MembershipTypeTranslations { get; set; }
    }

    public class MembershipTypeTranslation : TranslateBase
    {
        public int MembershipTypeId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
    }
}
