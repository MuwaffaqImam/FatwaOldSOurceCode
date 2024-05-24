using System.Collections.Generic;

namespace TeamWork.Core.Models.SystemDefinition
{
    public class TagModel : BaseModel
    {
        public bool Enabled { get; set; }
        public string TagName { get; set; }
        public List<TagTranslationModel> TagTranslationModels { get; set; }
    }

    public class TagTranslationModel : BaseTranslationModel
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
