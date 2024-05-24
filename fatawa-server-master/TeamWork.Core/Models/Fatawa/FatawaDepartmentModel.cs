using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaDepartmentModel : BaseModel
    {
        public string NodeNumber { get; set; }
        public string NodeMain { get; set; }
        public int ParentId { get; set; }
        public string NodeName { get; set; }
        public int NodeLevelNumber { get; set; }
        public int FatawaDepartmentTranslateId { get; set; }
        public List<FatawaDepartmentTranslationModel> FatawaDepartmentTranslations { get; set; }
        public int LanguageId { get; set; }
        public int Sort { get; set; }
    }

    public class FatawaDepartmentTranslationModel : BaseTranslationModel
    {
        public int FatawaDepartmentId { get; set; }
        public string NodeTranslationName { get; set; }
    }
}
