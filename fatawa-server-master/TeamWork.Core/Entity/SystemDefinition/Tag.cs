using System.Collections.Generic;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.SystemDefinition
{
    public class Tag : BaseEntity
    {
        public bool Enabled { get; set; }
        public virtual ICollection<TagTranslation> TagTranslations { get; set; }
    }

    public class TagTranslation : TranslateBase
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
    }
}
