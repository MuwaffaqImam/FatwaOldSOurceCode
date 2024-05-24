using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.DTO;
using TeamWork.Core.DTO.Chat;
using TeamWork.Core.Entity;
using TeamWork.Core.IServices.Chat;
using TeamWork.Core.Models.Chat;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Repository;

namespace TeamWork.Service.Chatting
{
    internal class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;

        internal ChatService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
        }

        public ChatMessageModel AddNewMassage(ChatMessageModel chatMessageModel)
        {
            try
            {
                Chat chat = _imapper.Map<Chat>(chatMessageModel);
                _unitOfWork.ChatRepository.Insert(chat);
                ChatMessageModel chatMessage = _imapper.Map<ChatMessageModel>(chat);

                if (chat.Id != 0)
                {
                    _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                    {
                        SenderId = chat.CreatedBy,
                        RecipientId = chat.RecipientId,
                        MessageText = "NewMessageWasAdded",
                        IsRead = false,
                        Deleted = false,
                        NotificationTypeId = (int)SystemEnum.NotificationType.Chatting,
                    });
                }

                return chatMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ChatMessageModel>> GetAllChattingMassage(int senderId, int recipientId)
        {
            try
            {
                IEnumerable<ChatMessageDTO> chatMessageDTO = await _unitOfWork.ChatRepository.GetAllChattingMassage(senderId, recipientId);
                IEnumerable<ChatMessageModel> chatMessageModels = _imapper.Map<IEnumerable<ChatMessageModel>>(chatMessageDTO);

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
                int countChatMessageModels = await _unitOfWork.ChatRepository.GetUnReadMessages(userId);
                return countChatMessageModels;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> MarkMessageAsRead(int userId, int messageId)
        {
            try
            {
                Chat chat = await _unitOfWork.ChatRepository.GetSingleAsync(messageId);

                if (chat.RecipientId == userId)
                {
                    chat.IsRead = true;
                    chat.DateRead = DateTime.Now;
                }

                return _unitOfWork.ChatRepository.Update(chat);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
