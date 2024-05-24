using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Security
{
    [Table("Roles")]
    public class Role : IdentityRole<int>
    {
        public int PowerLevel { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
