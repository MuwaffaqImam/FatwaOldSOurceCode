using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity.SystemDefinition;

namespace TeamWork.Core.Entity
{
    public class TranslateBase : BaseEntity
    {
        public int LanguageId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LanguageCode { get; set; }

        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
