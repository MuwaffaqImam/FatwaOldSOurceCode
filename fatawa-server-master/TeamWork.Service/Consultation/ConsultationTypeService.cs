using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Consultation;
using TeamWork.Core.IServices.Consultation;
using TeamWork.Core.Models.Consultation;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services.Consultation
{
    public class ConsultationTypeService : IConsultationTypeService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<ConsultationType> _consultationTypeRepository;

        public ConsultationTypeService(IMapper imapper, IRepository<ConsultationType> consultationTypeRepository)
        {
            _imapper = imapper;
            _consultationTypeRepository = consultationTypeRepository;
        }

        public List<ConsultationTypeModel> GetAllConsultationTypes()
        {
            try
            {
                List<ConsultationType> consultationTypes = _consultationTypeRepository.GetAll();
                return _imapper.Map<List<ConsultationTypeModel>>(consultationTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public ConsultationTypeModel GetConsultationType(int id)
        {
            try
            {
                ConsultationType consultationType = _consultationTypeRepository.GetSingle(id);
                return _imapper.Map<ConsultationTypeModel>(consultationType);
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public int AddConsultationType(ConsultationTypeModel model)
        {
            try
            {
                ConsultationType consultationType = _imapper.Map<ConsultationType>(model);
                return _consultationTypeRepository.Insert(consultationType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateConsultationType(ConsultationTypeModel model)
        {
            try
            {
                ConsultationType consultationType = _consultationTypeRepository.GetSingle(model.Id);
                return _consultationTypeRepository.Update(consultationType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteConsultationType(ConsultationTypeModel model)
        {
            try
            {
                ConsultationType consultationType = _consultationTypeRepository.GetSingle(model.Id);
                return _consultationTypeRepository.Delete(consultationType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
