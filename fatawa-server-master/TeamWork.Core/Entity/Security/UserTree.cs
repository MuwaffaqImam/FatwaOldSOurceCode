using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity.SystemDefinition;

namespace TeamWork.Core.Entity.Security
{
    [Table("UserTree")]
    public class UserTree : BaseEntity
    {
        public string NodeNumber { get; set; }
        public string NodeMain { get; set; }
        public int NodeLevelNumber { get; set; }
        public int mufitUserId { get; set; }
        [ForeignKey("mufitUserId")]
        public virtual User User { get; set; }

        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual UserTree UserTreeParent { get; set; }

        public int Sort { get; set; }
    }
}
