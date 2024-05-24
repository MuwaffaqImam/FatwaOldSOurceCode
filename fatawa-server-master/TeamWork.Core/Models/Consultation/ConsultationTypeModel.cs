using System.Collections.Generic;

namespace TeamWork.Core.Models.Consultation
{
    public class ConsultationTypeModel : BaseModel
    {
        public List<ConsultationTypeTranslationModel> ConsultationTypeTranslations { get; set; }
    }

    public class ConsultationTypeTranslationModel : BaseTranslationModel
    {
        public int ConsultationTypeId { get; set; }
        public string Name { get; set; }
    }
}
