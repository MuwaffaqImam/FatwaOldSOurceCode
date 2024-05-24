namespace TeamWork.Core.Models.Notification
{
    public class NotificationModel : BaseModel
    {
        public int ToUserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string FromConnectionId { get; set; }
        public string ToConnectionId { get; set; }
    }
}
