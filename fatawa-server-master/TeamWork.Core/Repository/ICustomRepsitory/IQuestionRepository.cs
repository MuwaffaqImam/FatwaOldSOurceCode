using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;

namespace TeamWork.Core.Repository.ICustomRepsitory
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<bool>UpdateCloseQuestion(int questionId, int statusUserId);
        Task<QuestionModel> GetQuestionById(int questionId);
    }
}
