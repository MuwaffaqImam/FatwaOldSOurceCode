using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Notification;

namespace TeamWork.Core.IServices.Notification
{
    public interface INotificationService
    {
        List<NotificationItemModel> GetAllNotificationItems();
        List<NotificationItemModel> GetAllNotificationItems(int userId);
        List<NotificationItemModel> GetAllUnreadNotificationItems();
        Task<IEnumerable<NotificationItemModel>> GetUnreadNotification(int userId);
        NotificationItemModel GetNotificationItem(int id);
        int AddNotificationItem(NotificationItemModel model);
        bool ReadNotificationItem(int notificationId); 
        bool UpdateReadNewNotification(int receiverId, int senderId, int notificationTypeId);
        bool DeleteNotificationItem(int id);
        Task<IEnumerable<NotificationItemModel>> GetNewQuestionsAndFatwa(string currentUserRole);
        bool UpdateReadNewQuestionsAndFatwa(string currentUserRole, int notificationTypeId);
    }
}
