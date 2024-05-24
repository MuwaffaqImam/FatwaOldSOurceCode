using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TeamWork.Core.Entity.Notification;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Repository.Notification;

namespace TeamWork.Repository.Repository.Notification
{
    internal class NotificationRepository : Repository<NotificationItem>, INotificationRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;

        internal NotificationRepository(IDbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public int AddNotificationItem(NotificationItemModel model)
        {
            NotificationItem notificationItem = _mapper.Map<NotificationItem>(model);
            notificationItem.NotificationItemTranslations = new List<NotificationItemTranslation>();

            List<NotificationTemplateTranslation> notificationTemplateTranslations = GetNotificationTemplateTranslation(model.NotificationTypeId);

            notificationTemplateTranslations.ForEach((NotificationTemplateTranslation notificationTemplateTranslation) =>
            {
                notificationItem.NotificationItemTranslations.Add(new NotificationItemTranslation
                {
                    LanguageCode = notificationTemplateTranslation.LanguageCode,
                    LanguageId = notificationTemplateTranslation.LanguageId,
                    MessageText = model.MessageText,
                });
            });

            Insert(notificationItem);
            _dbFactory.GetDataContext.SaveChanges();

            return notificationItem.Id;
        }

        public List<NotificationTemplateTranslation> GetNotificationTemplateTranslation(int notificationTypeId)
        {
            return _dbFactory.GetDataContext.NotificationTemplateTranslation.
                        AsNoTracking().Where(s => s.NotificationTypeId == notificationTypeId).ToList();
        }
    }
}
