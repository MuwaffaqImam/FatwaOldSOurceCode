using System.Collections.Generic;
using TeamWork.Core.Models;
using TeamWork.Core.Models.Consultation;

namespace TeamWork.Core.IServices.Consultation
{
    public interface IConsultationService
    {
        List<ConsultationModel> GetAllConsultations();
        int AddConsultation(ConsultationModel model);
        bool UpdateConsultation(ConsultationModel model);
        bool DeleteConsultation(int id);
        void ChangeLastSeen(int id);
        void IncreaseVistorsCount(int id);
        void ChangeStatus(int id, int nextStatusId);
    }
}
