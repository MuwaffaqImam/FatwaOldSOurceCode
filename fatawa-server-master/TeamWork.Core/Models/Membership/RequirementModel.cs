using System.Collections.Generic;

namespace TeamWork.Core.Models.Membership
{
    public class RequirementModel : BaseModel
    {
        public int MembershipTypeId { get; set; }
        public string MembershipTypeName { get; set; }

        public List<RequirementTranslationModel> RequirementTranslations { get; set; }
    }

    public class RequirementTranslationModel : BaseTranslationModel
    {
        public int RequirementId { get; set; }
        public string Text { get; set; }
    }
}
