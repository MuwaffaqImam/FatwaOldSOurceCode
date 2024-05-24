using System.ComponentModel.DataAnnotations;

namespace TeamWork.Core.Models.Consultation
{
    public class ConsultationDiscussionModel : BaseModel
    {
        [Required]
        public string MessageText { get; set; }

        [Required]
        public int ConsultationId { get; set; }
        public string ConsultationName { get; set; }
    }
}
