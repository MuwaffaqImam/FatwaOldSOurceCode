using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaTypeModel : BaseModel
    {
        public string Name { get; set; }
        public IList<FatawaTypeTranslationModel> FatawaTypeTranslations { get; set; }
    }

    public class FatawaTypeTranslationModel : BaseTranslationModel
    {
        public int FatawaDepartmentId { get; set; }
        public string Name { get; set; }
    }
}
