using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Chat;

namespace TeamWork.Core.IServices.Chat
{
    public interface IChatService
    {
        Task<IEnumerable<ChatMessageModel>> GetAllChattingMassage(int senderId, int recipientId);
        ChatMessageModel AddNewMassage(ChatMessageModel chatMessageModel);
        Task<int> GetUnReadMessages(int userId);
        Task<bool> MarkMessageAsRead(int userId, int messageId);
    }
}
