using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Fatawa
{
    [Table("Questions")]
    public class Question : BaseEntity
    {
        [Column(TypeName = "nvarchar(max)")]
        public string FatawaQuestion { get; set; }
        public int StatusId { get; set; }
        public int? StatusUserId { get; set; }

        public int TransferUserId { get; set; } = -1;
    }
}
