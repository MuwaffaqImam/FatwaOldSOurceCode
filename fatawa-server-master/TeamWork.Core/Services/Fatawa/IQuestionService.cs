using System.Threading.Tasks;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Sheard;

namespace TeamWork.IService.Fatawa
{
    public interface IQuestionService
    {
        int AddQuestion(QuestionModel model);
        Task<PaginationRecord<QuestionModel>> GetAllQuestionsAsync(int pageIndex, int pageSize, int questionStateId ,int MuftiId );
        Task<PaginationRecord<QuestionModel>> getAllQuestionsByStatusId(int pageIndex, int pageSize, int statusId, int userId, int MuftiId);
        bool UpdateCurrentStatusQuestion(int questionId, int statusId, int statusUserId);
        Task<QuestionModel> GetQuestionById(int questionId);
        Task<bool> UpdateCloseQuestion(int questionId, int statusUserId);
        Task<int> GetUserIdAddedQuestion(int questionId);
    }
}
