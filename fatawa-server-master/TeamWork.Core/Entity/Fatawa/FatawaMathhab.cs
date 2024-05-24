using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Fatawa
{
    public class FatawaMathhab : BaseEntity
    {
        public virtual ICollection<FatawaMathhabTranslation> FatawaMathhabTranslations { get; set; }
    }

    public class FatawaMathhabTranslation : TranslateBase
    {
        public int FatawaMathhabId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
