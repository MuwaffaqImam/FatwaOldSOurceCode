using System.Collections.Generic;

namespace TeamWork.Core.Models.Membership
{
    public class MembershipTypeModel : BaseModel
    {
        public List<MembershipTypeTranslationModel> MembershipTypeTranslations { get; set; }
    }

    public class MembershipTypeTranslationModel : BaseTranslationModel
    {
        public int MembershipTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
