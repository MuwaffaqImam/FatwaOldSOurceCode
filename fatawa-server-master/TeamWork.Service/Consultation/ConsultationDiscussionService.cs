using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Consultation;
using TeamWork.Core.IServices.Consultation;
using TeamWork.Core.Models.Consultation;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services.Consultation
{
    public class ConsultationDiscussionService : IConsultationDiscussionService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<ConsultationDiscussion> _consultationDiscussionRepository;

        public ConsultationDiscussionService(IMapper imapper, IRepository<ConsultationDiscussion> consultationDiscussionRepository)
        {
            _imapper = imapper;
            _consultationDiscussionRepository = consultationDiscussionRepository;
        }

        public List<ConsultationDiscussionModel> GetAllConsultationDiscussions()
        {
            try
            {
                List<ConsultationDiscussion> consultationDiscussions = _consultationDiscussionRepository.GetAll();
                return _imapper.Map<List<ConsultationDiscussionModel>>(consultationDiscussions);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ConsultationDiscussionModel GetConsultationDiscussion(int id)
        {
            try
            {
                ConsultationDiscussion consultationDiscussion = _consultationDiscussionRepository.GetSingle(id);
                return _imapper.Map<ConsultationDiscussionModel>(consultationDiscussion);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddConsultationDiscussion(ConsultationDiscussionModel model)
        {
            try
            {
                ConsultationDiscussion consultationDiscussion = _imapper.Map<ConsultationDiscussion>(model);
                return _consultationDiscussionRepository.Insert(consultationDiscussion);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteConsultationDiscussion(ConsultationDiscussionModel model)
        {
            try
            {
                ConsultationDiscussion consultationDiscussion = _consultationDiscussionRepository.GetSingle(model.Id);
                return _consultationDiscussionRepository.Delete(consultationDiscussion);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
