using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity
{
    [Table("Chat")]
    public class Chat : BaseEntity
    {
        public int RecipientId { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateRead { get; set; }
    }
}
