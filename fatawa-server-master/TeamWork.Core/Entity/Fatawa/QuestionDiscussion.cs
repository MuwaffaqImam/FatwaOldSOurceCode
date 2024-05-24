using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;
using TeamWork.Core.Entity.Security;

namespace TeamWork.Core.Entity.Fatawa
{
    [Table("QuestionsDiscussions")]
    public class QuestionDiscussion : BaseEntity
    {
        public int RepliedId { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string MessageText { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        public bool IsPublished { get; set; }

    }
}