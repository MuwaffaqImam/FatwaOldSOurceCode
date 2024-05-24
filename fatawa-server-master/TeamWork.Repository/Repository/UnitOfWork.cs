using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using TeamWork.Core.Email;
using TeamWork.Core.Entity;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Repository;
using TeamWork.Core.Repository.Chat;
using TeamWork.Core.Repository.ICustomRepsitory;
using TeamWork.Core.Repository.Notification;
using TeamWork.Repository.Repository.Chat;
using TeamWork.Repository.Repository.CustomRepository;
using TeamWork.Repository.Repository.Notification;
using TeamWork.Repository.Repository.Security;

namespace TeamWork.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly IDbFactory _dbFactory;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UnitOfWork(IDbFactory dbFactory, IMapper mapper,
                         UserManager<User> userManager, IConfiguration configuration,
                         SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _dbFactory = dbFactory;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }



        Lazy<IChatRepository> LazyChatRepository => new Lazy<IChatRepository>(() => new ChatRepository(_dbFactory));
        Lazy<ISecurityRepository> LazySecurityRepository => new Lazy<ISecurityRepository>(() =>
                    new SecurityRepository(_dbFactory, _mapper, _userManager, _signInManager, _configuration, _emailSender));
        Lazy<INotificationRepository> LazyNotificationRepository => new Lazy<INotificationRepository>(() => new NotificationRepository(_dbFactory, _mapper));
        Lazy<IFatawaRepository> LazyFatawaRepository => new Lazy<IFatawaRepository>(() => new FatawaRepository(_dbFactory, _mapper));
        Lazy<IQuestionRepository> LazyQuestionRepository => new Lazy<IQuestionRepository>(() => new QuestionRepository(_dbFactory, _mapper));
        Lazy<IQuestionDiscussionRepository> LazyQuestionDiscussionRepository => new Lazy<IQuestionDiscussionRepository>(() => new QuestionDiscussionRepository(_dbFactory, _mapper, _userManager));
        Lazy<IFatawaDepartmentRepository> LazyFatawaDepartmentRepository => new Lazy<IFatawaDepartmentRepository>(() => new FatawaDepartmentRepository(_dbFactory, _mapper));

        public IChatRepository ChatRepository => LazyChatRepository.Value;
        public ISecurityRepository SecurityRepository => LazySecurityRepository.Value;
        public INotificationRepository NotificationRepository => LazyNotificationRepository.Value;
        public IFatawaRepository FatawaRepository => LazyFatawaRepository.Value;
        public IQuestionRepository QuestionRepository => LazyQuestionRepository.Value;
        public IQuestionDiscussionRepository QuestionDiscussionRepository => LazyQuestionDiscussionRepository.Value;
        public IFatawaDepartmentRepository FatawaDepartmentRepository => LazyFatawaDepartmentRepository.Value;


        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbFactory);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public bool SaveChanges()
        {
            bool returnValue = true;
            using (var dbContextTransaction = _dbFactory.GetDataContext.Database.BeginTransaction())
            {
                try
                {
                    _dbFactory.GetDataContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    //Log Exception Handling message                      
                    returnValue = false;
                    dbContextTransaction.Rollback();
                }
            }

            return returnValue;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbFactory.GetDataContext.Dispose();
            }
        }
    }
}
