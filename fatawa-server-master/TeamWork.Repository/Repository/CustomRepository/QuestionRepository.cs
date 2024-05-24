using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository.ICustomRepsitory;
using static TeamWork.Core.DTO.SystemEnum;

namespace TeamWork.Repository.Repository.CustomRepository
{
    internal class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;

        internal QuestionRepository(IDbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<bool> UpdateCloseQuestion(int questionId, int statusUserId)
        {
            try
            {
                Question question = GetSingle(questionId);

                if (question != null)
                {
                    question.StatusId = (int)QuestionStatus.Closed;
                    question.StatusUserId = statusUserId;
                }

                Update(question);
                return await _dbFactory.GetDataContext.SaveChangesAsync() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<QuestionModel> GetQuestionById(int questionId)
        {
            try
            {
                QuestionModel questionModel = new QuestionModel();
                Question question = await GetSingleAsync(questionId);

                if (questionModel != null)
                {
                    questionModel = _mapper.Map<QuestionModel>(question);
                }

                return questionModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
