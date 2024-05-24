using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Consultation
{
    public class Consultation : BaseEntity
    {
        public int Order { get; set; }
        public int? ImageId { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }
        public int StatusId { get; set; }

        public int ConsultationTypeId { get; set; }
        [ForeignKey("ConsultationTypeId")]
        public virtual ConsultationType ConsultationType { get; set; }

        public virtual ICollection<ConsultationDiscussion> ConsultationDiscussion { get; set; }
        public virtual ICollection<ConsultationTranslation> ConsultationTranslations { get; set; }
    }

    public class ConsultationTranslation : TranslateBase
    {
        public int ConsultationId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }
    }
}
