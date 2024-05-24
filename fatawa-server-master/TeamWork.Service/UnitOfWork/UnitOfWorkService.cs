using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.IServices.Chat;
using TeamWork.Core.IServices.Notification;
using TeamWork.Core.IServices.Project;
using TeamWork.Core.IServices.SystemDefinition;
using TeamWork.Core.Repository;
using TeamWork.Core.Services;
using TeamWork.IService.Fatawa;
using TeamWork.Repository.Repository;
using TeamWork.Service.Chatting;
using TeamWork.Service.Fatawa;
using TeamWork.Service.Notification;
using TeamWork.Service.Project;
using TeamWork.Service.Security;
using TeamWork.Service.SystemDefinition;

namespace TeamWork.Service.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDbFactory _dbfactory;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public UnitOfWorkService(IUnitOfWork unitOfWork, IMapper mapper, IDbFactory dbFactory, Microsoft.Extensions.Configuration.IConfiguration iConfig, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbfactory = dbFactory;
            _configuration = iConfig;
            _userManager = userManager;

        }

        Lazy<IGeneralSettingsService> LazyGeneralSettingsService => new Lazy<IGeneralSettingsService>(new GeneralSettingsService(_unitOfWork, _mapper));
        Lazy<ILanguageService> LazyLanguageService => new Lazy<ILanguageService>(() => new LanguageService(_unitOfWork, _mapper));
        Lazy<ISecurityService> LazySecurityService => new Lazy<ISecurityService>(() => new SecurityService(_unitOfWork, _mapper,_configuration));
        Lazy<IChatService> LazyChatService => new Lazy<IChatService>(() => new ChatService(_unitOfWork, _mapper));
        Lazy<INotificationService> LazyNotificationService => new Lazy<INotificationService>(() => new NotificationService(_unitOfWork, _mapper,_userManager,_dbfactory));
        Lazy<IJobService> LazyJobService => new Lazy<IJobService>(() => new JobService(_unitOfWork, _mapper));
        Lazy<IFatawaService> LazyFatawaService => new Lazy<IFatawaService>(() => new FatawaService(_unitOfWork, _mapper));
        Lazy<IFatawaDepartmentService> LazyFatawaDepartmentService => new Lazy<IFatawaDepartmentService>(() => new FatawaDepartmentService(_unitOfWork, _mapper));
        Lazy<IQuestionService> LazyQuestionService => new Lazy<IQuestionService>(() => new QuestionService(_unitOfWork, _mapper, _dbfactory, _configuration));
        Lazy<IQuestionDiscussionService> LazyQuestionDiscussionService => new Lazy<IQuestionDiscussionService>(() => new QuestionDiscussionService(_unitOfWork, _mapper, _dbfactory, _userManager));
        Lazy<IFatawaTypeService> LazyFatawaTypeService => new Lazy<IFatawaTypeService>(() => new FatawaTypeService(_unitOfWork, _mapper));

        public IGeneralSettingsService GeneralSettingsService => LazyGeneralSettingsService.Value;
        public ILanguageService LanguageService => LazyLanguageService.Value;
        public ISecurityService SecurityService => LazySecurityService.Value;
        public IChatService ChatService => LazyChatService.Value;
        public INotificationService NotificationService => LazyNotificationService.Value;
        public IJobService JobService => LazyJobService.Value;
        public IFatawaService FatawaService => LazyFatawaService.Value;
        public IFatawaDepartmentService FatawaDepartmentService => LazyFatawaDepartmentService.Value;
        public IQuestionService QuestionService => LazyQuestionService.Value;
        public IQuestionDiscussionService QuestionDiscussionService => LazyQuestionDiscussionService.Value;
        public IFatawaTypeService FatawaTypeService => LazyFatawaTypeService.Value;
    }
}
