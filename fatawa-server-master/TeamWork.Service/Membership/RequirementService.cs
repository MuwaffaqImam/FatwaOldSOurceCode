using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Membership;
using TeamWork.Core.IServices.Membership;
using TeamWork.Core.Models.Membership;
using TeamWork.Core.Repository;

namespace TeamWork.Service.Membership
{
    public class RequirementService : IRequirementService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Requirement> _requirementRepository;

        public RequirementService(IMapper imapper, IRepository<Requirement> requirementRepository)
        {
            _imapper = imapper;
            _requirementRepository = requirementRepository;
        }

        public List<RequirementModel> GetAllRequirements()
        {
            try
            {
                List<Requirement> requirements = _requirementRepository.GetAll();
                return _imapper.Map<List<RequirementModel>>(requirements);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public RequirementModel GetRequirement(int id)
        {
            Requirement requirement = _requirementRepository.GetSingle(id);
            return _imapper.Map<RequirementModel>(requirement);
        }

        public int AddRequirement(RequirementModel model)
        {
            try
            {
                Requirement requirement = _imapper.Map<Requirement>(model);
                return _requirementRepository.Insert(requirement);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateRequirement(RequirementModel model)
        {
            try
            {
                Requirement requirement = _requirementRepository.GetSingle(model.Id);
                return _requirementRepository.Update(requirement);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteRequirement(int id)
        {
            try
            {
                Requirement Requirement = _requirementRepository.GetSingle(id);
                return _requirementRepository.Delete(Requirement);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
