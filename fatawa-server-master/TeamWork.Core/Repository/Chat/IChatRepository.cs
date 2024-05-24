using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.DTO.Chat;

namespace TeamWork.Core.Repository.Chat
{
    public interface IChatRepository : IRepository<TeamWork.Core.Entity.Chat>
    {
        Task<IEnumerable<ChatMessageDTO>> GetAllChattingMassage(int senderId, int recipientId);
        Task<int> GetUnReadMessages(int userId);
    }
}
