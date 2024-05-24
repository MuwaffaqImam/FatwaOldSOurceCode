using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Membership
{
    public class Requirement : BaseEntity
    {
        public virtual ICollection<MembershipType> MembershipTypes { get; set; }
        public virtual ICollection<RequirementTranslation> RequirementTranslations { get; set; }
    }

    public class RequirementTranslation : TranslateBase
    {
        public int RequirementId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Text { get; set; }
    }
}
