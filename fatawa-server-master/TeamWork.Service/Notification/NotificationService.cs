using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Notification;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.IServices.Notification;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Repository;
using TeamWork.Repository.Repository;
using static TeamWork.Core.DTO.SystemEnum;

namespace TeamWork.Service.Notification
{
    internal class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;
        private readonly UserManager<User> _userManager;
        private readonly IDbFactory _dbFactory;

        internal NotificationService(IUnitOfWork unitOfWork, IMapper imapper, UserManager<User> userManager, IDbFactory dbFactory)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
            _userManager = userManager;
            _dbFactory = dbFactory;
        }

        public List<NotificationItemModel> GetAllNotificationItems()
        {
            try
            {
                List<NotificationItem> notificationItems = _unitOfWork.NotificationRepository.GetAll();
                return _imapper.Map<List<NotificationItemModel>>(notificationItems);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public List<NotificationItemModel> GetAllNotificationItems(int userId)
        {
            try
            {
                List<NotificationItem> notificationItems = _unitOfWork.NotificationRepository
                    .GetWhere(s => s.RecipientId == userId).ToList();

                return _imapper.Map<List<NotificationItemModel>>(notificationItems);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public List<NotificationItemModel> GetAllUnreadNotificationItems()
        {
            try
            {
                List<NotificationItem> notificationItems = _unitOfWork.NotificationRepository
                    .GetWhere(s => s.IsRead == false).ToList();

                return _imapper.Map<List<NotificationItemModel>>(notificationItems);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<NotificationItemModel>> GetUnreadNotification(int userId)
        {
            try
            {
                User user = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == userId);



                var rolesForUser = await _userManager.GetRolesAsync(user);

                IEnumerable<NotificationItem> notificationItems;


                if (rolesForUser[0] != "Guest")
                {
                    notificationItems = await _unitOfWork.NotificationRepository
                   .GetAllIncludingWithPredicate(
                   new Expression<Func<NotificationItem, object>>[] { x => x.NotificationItemTranslations }
                   , new Expression<Func<NotificationItem, bool>>[] { s => s.RecipientId == userId });
                }
                else
                {
                    notificationItems = await _unitOfWork.NotificationRepository
                   .GetAllIncludingWithPredicate(
                   new Expression<Func<NotificationItem, object>>[] { x => x.NotificationItemTranslations }
                   , new Expression<Func<NotificationItem, bool>>[] { s => s.IsRead == false & s.RecipientId == userId });//& s.IsPublished == true 
                }



                return _imapper.Map<List<NotificationItemModel>>(notificationItems);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public NotificationItemModel GetNotificationItem(int id)
        {
            try
            {
                NotificationItem notificationItem = _unitOfWork.NotificationRepository.GetSingle(id);
                return _imapper.Map<NotificationItemModel>(notificationItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddNotificationItem(NotificationItemModel model)
        {
            try
            {
                return _unitOfWork.NotificationRepository.AddNotificationItem(model);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool ReadNotificationItem(int notificationId)
        {
            try
            {
                NotificationItem notificationItem = _unitOfWork.GetRepository<NotificationItem>().GetSingle(notificationId);
                notificationItem.IsRead = true;
                _unitOfWork.GetRepository<NotificationItem>().Update(notificationItem);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateReadNewNotification(int receiverId, int senderId, int notificationTypeId)
        {
            try
            {
                List<NotificationItem> notificationItems = _unitOfWork.GetRepository<NotificationItem>()
                    .GetWhere(s => s.IsRead == false && s.CreatedBy == senderId &&
                                   (s.RecipientId == receiverId) && s.NotificationTypeId == notificationTypeId).ToList();

                notificationItems.ForEach(read => read.IsRead = true);

                _unitOfWork.GetRepository<NotificationItem>().UpdateRange(notificationItems);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteNotificationItem(int id)
        {
            try
            {
                NotificationItem notificationItem = _unitOfWork.GetRepository<NotificationItem>().GetSingle(id);
                _unitOfWork.GetRepository<NotificationItem>().Delete(notificationItem);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private List<NotificationTemplateTranslation> GetNotificationTemplateTranslation(int notificationTypeId)
        {
            return _unitOfWork.GetRepository<NotificationTemplateTranslation>().GetWhere(s => s.NotificationTypeId == notificationTypeId).ToList();
        }

        public async Task<IEnumerable<NotificationItemModel>> GetNewQuestionsAndFatwa(string currentUserRole)
        {
            try
            {
                IEnumerable<NotificationItemModel> NewQuestionsnotificationModel = new List<NotificationItemModel>();
                RolesType enumRoleName = (RolesType)Enum.Parse(typeof(RolesType), currentUserRole);

                IEnumerable<NotificationItem> NewQuestionsnotification = await _unitOfWork.GetRepository<NotificationItem>()
                    .GetAllIncludingWithPredicate(
                    new Expression<Func<NotificationItem, object>>[] { x => x.NotificationItemTranslations }
                    , new Expression<Func<NotificationItem, bool>>[] { s => s.IsRead == false & s.RecipientRoleId == (int)enumRoleName });

                NewQuestionsnotificationModel = _imapper.Map<List<NotificationItemModel>>(NewQuestionsnotification);

                return NewQuestionsnotificationModel;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateReadNewQuestionsAndFatwa(string currentUserRole, int notificationTypeId)
        {
            try
            {
                RolesType enumRoleName = (RolesType)Enum.Parse(typeof(RolesType), currentUserRole);

                List<NotificationItem> notificationItems = _unitOfWork.GetRepository<NotificationItem>()
                    .GetWhere(s => s.IsRead == false &&
                                   (s.RecipientRoleId == (int)enumRoleName) && s.NotificationTypeId == notificationTypeId).ToList();

                notificationItems.ForEach(read => read.IsRead = true);

                _unitOfWork.GetRepository<NotificationItem>().UpdateRange(notificationItems);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
