using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Selection
{
    public class SelectionItem : BaseEntity
    {
        public int Order { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }

        public int SelectionTypeId { get; set; }
        [ForeignKey("SelectionTypeId")]
        public virtual SelectionType SelectionType { get; set; }

        public virtual ICollection<SelectionItemTranslation> SelectionItemTranslations { get; set; }
    }

    public class SelectionItemTranslation : TranslateBase
    {
        public int SelectionItemId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; }
    }
}
