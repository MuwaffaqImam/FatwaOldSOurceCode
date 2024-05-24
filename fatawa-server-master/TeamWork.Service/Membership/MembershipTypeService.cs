using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Membership;
using TeamWork.Core.IServices.Membership;
using TeamWork.Core.Models.Membership;
using TeamWork.Core.Repository;

namespace TeamWork.Service.MembershipTypeship
{
    public class MembershipTypeService : IMembershipTypeService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<MembershipType> _membershipTypeRepository;

        public MembershipTypeService(IMapper imapper, IRepository<MembershipType> membershipTypeRepository)
        {
            _imapper = imapper;
            _membershipTypeRepository = membershipTypeRepository;
        }

        public List<MembershipTypeModel> GetAllMembershipTypes()
        {
            try
            {
                List<MembershipType> membershipTypes = _membershipTypeRepository.GetAll();
                return _imapper.Map<List<MembershipTypeModel>>(membershipTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public MembershipTypeModel GetMembershipType(int id)
        {
            MembershipType membershipType = _membershipTypeRepository.GetSingle(id);
            return _imapper.Map<MembershipTypeModel>(membershipType);
        }

        public int AddMembershipType(MembershipTypeModel model)
        {
            try
            {
                MembershipType membershipType = _imapper.Map<MembershipType>(model);
                return _membershipTypeRepository.Insert(membershipType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateMembershipType(MembershipTypeModel model)
        {
            try
            {
                MembershipType membershipType = _membershipTypeRepository.GetSingle(model.Id);
                return _membershipTypeRepository.Update(membershipType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteMembershipType(int id)
        {
            try
            {
                MembershipType MembershipType = _membershipTypeRepository.GetSingle(id);
                return _membershipTypeRepository.Delete(MembershipType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
