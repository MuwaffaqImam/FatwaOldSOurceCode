using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Fatawa
{
    public class FatawaDepartment : BaseEntity
    {
        public string NodeNumber { get; set; }
        public string NodeMain { get; set; }
        public int NodeLevelNumber { get; set; }
        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual FatawaDepartment FatawaDepartmentParent { get; set; }
        public virtual ICollection<FatawaDepartmentTranslation> FatawaDepartmentTranslations { get; set; }

        public int Sort { get; set; }
    }

    public class FatawaDepartmentTranslation : TranslateBase
    {
        public int FatawaDepartmentId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string NodeName { get; set; }
    }
}