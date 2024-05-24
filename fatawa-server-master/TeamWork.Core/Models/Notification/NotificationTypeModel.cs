using System.Collections.Generic;

namespace TeamWork.Core.Models.Notification
{
    public class NotificationTypeModel : BaseModel
    {
        public int Name { get; set; }
        public string Description { get; set; }

        public List<NotificationItemModel> NotificationItems { get; set; }
    }
}
