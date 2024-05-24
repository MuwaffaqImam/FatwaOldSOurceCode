using System.Collections.Generic;

namespace TeamWork.Core.Models.SystemDefinition
{
    public class CountryModel : BaseModel
    {
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public List<CountryTranslationModel> CountryTranslations { get; set; }
    }

    public class CountryTranslationModel : BaseTranslationModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
