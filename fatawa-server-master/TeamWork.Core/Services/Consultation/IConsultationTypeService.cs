using System.Collections.Generic;
using TeamWork.Core.Models.Consultation;

namespace TeamWork.Core.IServices.Consultation
{
    public interface IConsultationTypeService
    {
        List<ConsultationTypeModel> GetAllConsultationTypes();
        ConsultationTypeModel GetConsultationType(int id);
        int AddConsultationType(ConsultationTypeModel model);
        bool UpdateConsultationType(ConsultationTypeModel model);
        bool DeleteConsultationType(ConsultationTypeModel model);
    }
}
