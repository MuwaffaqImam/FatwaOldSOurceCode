using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Consultation
{
    public class ConsultationDiscussion : BaseEntity
    {
        [Column(TypeName = "nvarchar(max)")]
        public string MessageText { get; set; }

        public int ConsultationId { get; set; }
        [ForeignKey("ConsultationId")]
        public virtual Consultation Consultation { get; set; }
    }
}
