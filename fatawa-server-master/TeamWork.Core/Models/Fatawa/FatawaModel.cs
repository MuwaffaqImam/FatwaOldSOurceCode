using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaModel : BaseModel
    {
        public int? FatwaId { get; set; }
        public string Name { get; set; }
        public string FatawaQuestion { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public int MuftiId { get; set; }
        public string MuftiName { get; set; }
        public string TranslatorName { get; set; }
        public string Url { get; set; }

        public int FatawaTypeId { get; set; }
        public int FatawaTypeName { get; set; }

        public int FatawaDepartmentId { get; set; }
        public string FatawaDepartmentName { get; set; }

        public int? StatusId { get; set; }
        public string StatusName { get; set; }

        public int FatawaMathhabId { get; set; }
        public string MathhabName { get; set; }

        public List<FatawaTranslationModel> FatawaTranslations { get; set; }
        public List<TagModel> Tags { get; set; }

        public int LanguageId { get; set; }

        public int? QuestionId { get; set; }
    }

    public class FatawaTranslationModel : BaseTranslationModel
    {
        public int FatawaId { get; set; }
        public string Name { get; set; }
        public string FatawaQuestion { get; set; }
        public string Description { get; set; }
    }

    public class TagModel
    {
        public string Name { get; set; }
    }
}
