using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Fatawa
{
    public class FatawaStatus : BaseEntity
    {
        public virtual ICollection<FatawaStatusTranslation> FatawaStatusTranslations { get; set; }
    }

    public class FatawaStatusTranslation : TranslateBase
    {
        public int FatawaStatusId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
