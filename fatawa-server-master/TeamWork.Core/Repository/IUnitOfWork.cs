using TeamWork.Core.Entity;
using TeamWork.Core.Repository.Chat;
using TeamWork.Core.Repository.ICustomRepsitory;
using TeamWork.Core.Repository.Notification;

namespace TeamWork.Core.Repository
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : BaseEntity;
        IChatRepository ChatRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IFatawaRepository FatawaRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IQuestionDiscussionRepository QuestionDiscussionRepository { get; }
        IFatawaDepartmentRepository FatawaDepartmentRepository { get; }

        bool SaveChanges();
    }
}
