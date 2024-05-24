using System.Collections.Generic;

namespace TeamWork.Core.Models.Selection
{
    public class SelectionTypeModel : BaseModel
    {
        public List<SelectionTypeTranslationModel> SelectionTypeTranslations { get; set; }
    }

    public class SelectionTypeTranslationModel : BaseTranslationModel
    {
        public int SelectionTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
