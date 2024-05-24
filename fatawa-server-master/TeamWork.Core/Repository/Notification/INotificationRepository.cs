using TeamWork.Core.Entity.Notification;
using TeamWork.Core.Models.Notification;

namespace TeamWork.Core.Repository.Notification
{
    public interface INotificationRepository : IRepository<NotificationItem>
    {
        int AddNotificationItem(NotificationItemModel model);
    }
}
