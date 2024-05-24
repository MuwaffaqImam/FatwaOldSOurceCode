using System.Collections.Generic;

namespace TeamWork.Core.Models.SystemDefinition
{
    public class LanguageModel : BaseModel
    {
        public string LanguageCode { get; set; }
        public string LanguageDirection { get; set; }
        public string? LanguageFlag { get; set; }
        public string LanguageDefaultDisply { get; set; }
        public int LanguageId { get; set; }
        public virtual List<LanguageTranslationModel> LanguageTranslations { get; set; }
    }

    public class LanguageTranslationModel : BaseTranslationModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
