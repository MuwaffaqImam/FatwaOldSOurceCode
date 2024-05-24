using TeamWork.Core.IServices.Chat;
using TeamWork.Core.IServices.Notification;
using TeamWork.Core.IServices.Project;
using TeamWork.Core.IServices.SystemDefinition;
using TeamWork.IService.Fatawa;

namespace TeamWork.Core.Services
{
    public interface IUnitOfWorkService
    {
        IGeneralSettingsService GeneralSettingsService { get; }
        ILanguageService LanguageService { get; }
        IChatService ChatService { get; }
        INotificationService NotificationService { get; }
        ISecurityService SecurityService { get; }
        IJobService JobService { get; }
        IFatawaService FatawaService { get; }
        IFatawaDepartmentService FatawaDepartmentService { get; }
        IFatawaTypeService FatawaTypeService { get; }
        IQuestionDiscussionService QuestionDiscussionService { get; }
        IQuestionService QuestionService { get; }
    }
}
