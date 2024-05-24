using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.Core.Repository.ICustomRepsitory
{
    public interface IQuestionDiscussionRepository : IRepository<QuestionDiscussion>
    {
        Task<IEnumerable<QuestionDiscussionModel>> GetAllConversationAsync(int senderId, int receiverId, int QuestionId);

        Task<int> GetUnReadMessages(int userId);
    }
}
