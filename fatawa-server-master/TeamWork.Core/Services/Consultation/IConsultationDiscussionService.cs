using System.Collections.Generic;
using TeamWork.Core.Models.Consultation;

namespace TeamWork.Core.IServices.Consultation
{
    public interface IConsultationDiscussionService
    {
        List<ConsultationDiscussionModel> GetAllConsultationDiscussions();
        ConsultationDiscussionModel GetConsultationDiscussion(int id);
        int AddConsultationDiscussion(ConsultationDiscussionModel model);
        bool DeleteConsultationDiscussion(ConsultationDiscussionModel model);
    }
}
