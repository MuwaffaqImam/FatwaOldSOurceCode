using System.Collections.Generic;
using TeamWork.Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamWork.Core.Entity.Consultation
{
    public class ConsultationType : BaseEntity
    {
        public virtual ICollection<Consultation> Consultations { get; set; }
        public virtual ICollection<ConsultationTypeTranslation> ConsultationTypeTranslations { get; set; }
    }

    public class ConsultationTypeTranslation : TranslateBase
    {
        public int ConsultationTypeId { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; }
    }
}
