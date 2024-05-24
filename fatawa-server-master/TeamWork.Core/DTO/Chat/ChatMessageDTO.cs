using System;
using System.Collections.Generic;
using System.Text;

namespace TeamWork.Core.DTO.Chat
{
    public class ChatMessageDTO : BaseDTO
    {
        public string? SenderName { get; set; }
        public string? SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientPhotoUrl { get; set; }
        public string MessageText { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? DateRead { get; set; }
    }
}
