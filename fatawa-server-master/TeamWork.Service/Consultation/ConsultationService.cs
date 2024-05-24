using AutoMapper;
using System;
using System.Collections.Generic;
using TeamWork.Core.IServices.Consultation;
using TeamWork.Core.Models.Consultation;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services.Consultation
{
    public class ConsultationService : IConsultationService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Entity.Consultation.Consultation> _consultationRepository;

        public ConsultationService(IMapper imapper, IRepository<Entity.Consultation.Consultation> consultationRepository)
        {
            _imapper = imapper;
            _consultationRepository = consultationRepository;
        }

        public List<ConsultationModel> GetAllConsultations()
        {
            try
            {
                List<Entity.Consultation.Consultation> consultations = _consultationRepository.GetAll();
                return _imapper.Map<List<ConsultationModel>>(consultations);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddConsultation(ConsultationModel model)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _imapper.Map<Entity.Consultation.Consultation>(model);
                return _consultationRepository.Insert(consultation);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateConsultation(ConsultationModel model)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _consultationRepository.GetSingle(model.Id);
                consultation.ConsultationTypeId = model.ConsultationTypeId;
                consultation.Order = model.Order;
                consultation.Tags = model.Tags;
                return _consultationRepository.Update(consultation);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteConsultation(int id)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _consultationRepository.GetSingle(id);
                return _consultationRepository.Delete(consultation);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void ChangeLastSeen(int id)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _consultationRepository.GetSingle(id);
                consultation.LastSeen = DateTime.UtcNow;
                _consultationRepository.Update(consultation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void IncreaseVistorsCount(int id)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _consultationRepository.GetSingle(id);
                consultation.Visitors = consultation.Visitors + 1;
                _consultationRepository.Update(consultation);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ChangeStatus(int id, int nextStatusId)
        {
            try
            {
                Entity.Consultation.Consultation consultation = _consultationRepository.GetSingle(id);
                consultation.StatusId = nextStatusId;
                _consultationRepository.Update(consultation);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
