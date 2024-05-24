using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Library
{
    public class LibraryItem : BaseEntity
    {
        public int Order { get; set; }
        public decimal Version { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string URLDownLoad { get; set; }
        //ToDo:enum
        public int ExtensionType { get; set; }

        public int ItemTypeId { get; set; }
        [ForeignKey("ItemTypeId")]
        public virtual LibraryType LibraryType { get; set; }

        public virtual ICollection<LibraryItemTranslation> LibraryItemTranslations { get; set; }
    }

    public class LibraryItemTranslation : TranslateBase
    {
        public int LibraryItemId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string ItemName { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string AuthorName { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string AuditingName { get; set; }
    }
}
