using System;

namespace TeamWork.Core.Models.Fatawa
{
    public class QuestionDiscussionModel : BaseModel
    {
        public int? SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderPhotoUrl { get; set; }
        public string MessageText { get; set; }
        public int QuestionId { get; set; }
        public int RepliedId { get; set; }
        public string? RepliedName { get; set; }
        public string? RepliedPhotoUrl { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public bool IsPublished { get; set; }

    }
}
