using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Notification
{
    public class NotificationTemplate : BaseEntity
    {
        public int NotificationTypeId { get; set; }
        public virtual ICollection<NotificationTemplateTranslation> NotificationTemplateTranslations { get; set; }
    }

    public class NotificationTemplateTranslation : TranslateBase
    {
        public int NotificationTypeId { get; set; }
        public int NotificationTemplateId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
