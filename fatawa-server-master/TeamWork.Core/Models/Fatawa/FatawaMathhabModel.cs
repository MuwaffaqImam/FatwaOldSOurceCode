using System.Collections.Generic;

namespace TeamWork.Core.Models.Fatawa
{
    public class FatawaMathhabModel : BaseModel
    {
        public string Name { get; set; }
        public IList<FatawaMathhabTranslationModel> FatawaMathhabTranslations { get; set; }
    }

    public class FatawaMathhabTranslationModel : BaseTranslationModel
    {
        public int FatawaMathhabtId { get; set; }
        public string Name { get; set; }
    }
}
