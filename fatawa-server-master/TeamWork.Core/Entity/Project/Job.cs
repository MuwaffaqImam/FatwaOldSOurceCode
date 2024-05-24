using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity
{
    [Table("Jobs")]
    public class Job : BaseEntity
    {
        [Required, MaxLength(150)]
        public string JobName { get; set; }
    }
}
