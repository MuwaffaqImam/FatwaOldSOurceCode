using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity.Security;

namespace TeamWork.Core.Entity.Fatawa
{
    public class Fatawa : BaseEntity
    {
        public int Order { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }

        public string ImageUrl { get; set; }

        public int FatawaTypeId { get; set; }
        [ForeignKey("FatawaTypeId")]
        public virtual FatawaType FatawaType { get; set; }

        public int FatawaDepartmentId { get; set; }
        [ForeignKey("FatawaDepartmentId")]
        public virtual FatawaDepartment FatawaDepartment { get; set; }

        public int MuftiId { get; set; }
        [ForeignKey("MuftiId")]
        public virtual User Mufti { get; set; }

        public int FatawaMathhabId { get; set; }
        [ForeignKey("FatawaMathhabId")]
        public virtual FatawaMathhab FatawaMathhab { get; set; }

        public virtual ICollection<FatawaTranslation> FatawaTranslations { get; set; }
        public int? QuestionId { get; set; }

        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual FatawaStatus FatawaStatus { get; set; }
    }

    public class FatawaTranslation : TranslateBase
    {
        public int FatawaId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string FatawaQuestion { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string TranslatorName { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string TagName { get; set; }
    }
}
