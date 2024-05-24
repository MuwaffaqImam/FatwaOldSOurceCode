using System.Collections.Generic;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.SystemDefinition
{
    public class Country : BaseEntity
    {
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public virtual ICollection<CountryTranslation> CountryTranslations { get; set; }
    }

    public class CountryTranslation : TranslateBase
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
