using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.IService.Fatawa
{
    public interface IQuestionDiscussionService
    {
        Task<List<QuestionDiscussionModel>> GetAllQuestionDiscussions();
        Task<QuestionDiscussionModel> GetQuestionDiscussion(int id);
        Task<IEnumerable<QuestionDiscussionModel>> GetAllConversationAsync(int senderId, int receiverId, int QuestionId);
        
        QuestionDiscussionModel AddQuestionDiscussion(QuestionDiscussionModel questionDiscussionModel);
        Task<int> GetUnReadMessages(int userId);
        bool DeleteQuestionDiscussion(int id);
        Task<bool> MarkMessageAsRead(int userId,int messageId);
        bool PublishQuestiondiscussion(int questionId);
    }
}
