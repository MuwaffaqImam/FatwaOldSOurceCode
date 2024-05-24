using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamWork.Core.DTO.Chat;
using TeamWork.Core.Repository.Chat;

namespace TeamWork.Repository.Repository.Chat
{
    internal class ChatRepository : Repository<TeamWork.Core.Entity.Chat>, IChatRepository
    {
        private readonly IDbFactory _dbFactory;

        internal ChatRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllChattingMassage(int senderId, int recipientId)
        {
            try
            {
                IEnumerable<ChatMessageDTO> chatMessageModels = await (from chat in _dbFactory.GetDataContext.Chat
                                                                       join userSender in _dbFactory.GetDataContext.Users on chat.CreatedBy equals userSender.Id
                                                                       join userRecipien in _dbFactory.GetDataContext.Users on chat.CreatedBy equals userRecipien.Id
                                                                       where chat.RecipientId == senderId && chat.CreatedBy == recipientId ||
                                                                       chat.RecipientId == recipientId && chat.CreatedBy == senderId
                                                                       select new ChatMessageDTO
                                                                       {
                                                                           Id = chat.Id,
                                                                           CreatedBy = chat.CreatedBy,
                                                                           CreatedDate = chat.CreatedDate,
                                                                           UpdatedBy = chat.UpdatedBy,
                                                                           UpdatedDate = chat.UpdatedDate,
                                                                           MessageText = chat.MessageText,
                                                                           IsRead = chat.IsRead,
                                                                           DateRead = chat.DateRead,
                                                                           SenderName = userSender.UserName,
                                                                           SenderPhotoUrl = userSender.PhotoURL,
                                                                           RecipientId = chat.RecipientId,
                                                                           RecipientName = userRecipien.UserName,
                                                                           RecipientPhotoUrl = userRecipien.PhotoURL
                                                                       }
                                                                   ).OrderByDescending(o => o.CreatedDate).ToListAsync();

                return chatMessageModels;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetUnReadMessages(int userId)
        {
            try
            {
                IEnumerable<TeamWork.Core.Entity.Chat> chatMessageModels = await _dbFactory.GetDataContext.Chat.Where(m => m.IsRead == false && m.RecipientId == userId).ToListAsync();
                return chatMessageModels.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
