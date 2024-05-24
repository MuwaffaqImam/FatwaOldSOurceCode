using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.SystemDefinition
{
    public class Language : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string LanguageCode { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string LanguageDirection { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string LanguageFlag { get; set; }
        public string LanguageDefaultDisply { get; set; }
        public virtual ICollection<LanguageTl> LanguagesTl { get; set; }
    }
    public class LanguageTl : TranslateBase
    {
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
    }
}
