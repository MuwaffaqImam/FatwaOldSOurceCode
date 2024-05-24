using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Fatawa
{
    public class FatawaType : BaseEntity
    {
        public virtual ICollection<FatawaTypeTranslation> FatawaTypeTranslations { get; set; }
    }

    public class FatawaTypeTranslation : TranslateBase
    {
        public int FatawaTypeId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
