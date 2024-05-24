using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Selection
{
    public class SelectionType : BaseEntity
    {
        public virtual ICollection<SelectionTypeTranslation> SelectionTypeTranslations { get; set; }
    }

    public class SelectionTypeTranslation : TranslateBase
    {
        public int SelectionTypeId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string TypeName { get; set; }
    }
}
