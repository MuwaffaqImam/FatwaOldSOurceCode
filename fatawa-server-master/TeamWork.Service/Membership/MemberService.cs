using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Membership;
using TeamWork.Core.IServices.Membership;
using TeamWork.Core.Models.Membership;
using TeamWork.Core.Repository;

namespace TeamWork.Service.Membership
{
    public class MemberService : IMemberService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Member> _memberRepository;

        public MemberService(IMapper imapper, IRepository<Member> memberRepository)
        {
            _imapper = imapper;
            _memberRepository = memberRepository;
        }

        public List<MemberModel> GetAllMembers()
        {
            try
            {
                List<Member> members = _memberRepository.GetAll();
                return _imapper.Map<List<MemberModel>>(members);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public MemberModel GetMember(int id)
        {
            try
            {
                Member member = _memberRepository.GetSingle(id);
                return _imapper.Map<MemberModel>(member);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddMember(MemberModel model)
        {
            try
            {
                Member member = _imapper.Map<Member>(model);
                return _memberRepository.Insert(member);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateMember(MemberModel model)
        {
            try
            {
                Member member = _memberRepository.GetSingle(model.Id);
                member.FirstName = model.FirstName;
                member.SecondName = model.SecondName;
                member.LastName = model.LastName;
                member.ImageId = model.ImageId;
                member.Gender = model.Gender;
                member.BirthDate = model.BirthDate;
                member.ExternalCourses = model.ExternalCourses;
                member.Certificates = model.Certificates;
                member.MembershipTypeId = model.MembershipTypeId;
                member.Researchs = model.Researchs;
                return _memberRepository.Update(member);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteMember(int id)
        {
            try
            {
                Member member = _memberRepository.GetSingle(id);
                return _memberRepository.Delete(member);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
