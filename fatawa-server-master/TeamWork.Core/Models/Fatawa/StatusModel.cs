using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class StatusModel : BaseModel
    {
        public string Name { get; set; }
        public IList<StatusTranslationModel> StatusTranslations { get; set; }
    }

    public class StatusTranslationModel : BaseTranslationModel
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
}
