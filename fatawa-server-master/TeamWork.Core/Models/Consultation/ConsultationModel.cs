using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamWork.Core.Models.Consultation
{
    public class ConsultationModel : BaseModel
    {
        [Required]
        public int Order { get; set; }
        public int? ImageId { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }

        [Required]
        public string Tags { get; set; }
        [Required]
        public int StatusId { get; set; }

        [Required]
        public int ConsultationTypeId { get; set; }
        public int ConsultationTypeName { get; set; }

        public List<ConsultationTranslationModel> ConsultationTranslations { get; set; }
    }

    public class ConsultationTranslationModel : BaseTranslationModel
    {
        public int ConsultationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
