using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Library
{
    public class LibraryType : BaseEntity
    {
        public virtual ICollection<LibraryTypeTranslation> LibraryTypeTranslations { get; set; }
    }

    public class LibraryTypeTranslation : TranslateBase
    {
        public int LibraryTypetId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string TypeName { get; set; }
    }
}
