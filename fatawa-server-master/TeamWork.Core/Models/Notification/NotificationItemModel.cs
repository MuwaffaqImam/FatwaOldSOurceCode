namespace TeamWork.Core.Models.Notification
{
    public class NotificationItemModel : BaseModel
    {
        public string MessageText { get; set; }
        public int RecipientId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public bool IsRead { get; set; }
        public bool Deleted { get; set; }
        public int NotificationTypeId { get; set; }
        public int ReferenceMassageId { get; set; }
        public int RecipientRoleId { get; set; }
      //  public bool IsPublished { get; set; }


    }
}
