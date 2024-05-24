using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamWork.Core.DTO;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Repository;
using TeamWork.Core.Sheard;
using TeamWork.IService.Fatawa;
using TeamWork.Repository.Repository;

namespace TeamWork.Service.Fatawa
{
    internal class QuestionDiscussionService : IQuestionDiscussionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;
        private readonly IDbFactory _dbFactory;
        private readonly UserManager<User> _userManager;


        internal QuestionDiscussionService(IUnitOfWork unitOfWork, IMapper imapper, IDbFactory dbFactory, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
            _dbFactory = dbFactory;
            _userManager = userManager;
        }

        public async Task<List<QuestionDiscussionModel>> GetAllQuestionDiscussions()
        {
            try
            {
                List<QuestionDiscussion> questionDiscussions = await _unitOfWork.GetRepository<QuestionDiscussion>().GetAllAsync();
                return _imapper.Map<List<QuestionDiscussionModel>>(questionDiscussions);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<QuestionDiscussionModel> GetQuestionDiscussion(int id)
        {
            try
            {
                QuestionDiscussion questionDiscussion = await _unitOfWork.GetRepository<QuestionDiscussion>().GetSingleAsync(id);
                return _imapper.Map<QuestionDiscussionModel>(questionDiscussion);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<PaginationRecord<QuestionDiscussionModel>> GetInboxMessagesAsync(int pageIndex, int pageSize, int userId)
        {
            try
            {
                PaginationRecord<QuestionDiscussion> questionDiscussion = await _unitOfWork.GetRepository<QuestionDiscussion>().GetAllAsync(pageIndex, pageSize, x => x.Id, new Expression<Func<QuestionDiscussion, bool>>[] { x => x.CreatedBy == userId }, OrderBy.Descending);

                PaginationRecord<QuestionDiscussionModel> paginationRecordModel = new PaginationRecord<QuestionDiscussionModel>
                {
                    DataRecord = _imapper.Map<IEnumerable<QuestionDiscussionModel>>(questionDiscussion.DataRecord),
                    CountRecord = questionDiscussion.CountRecord,
                };

                return paginationRecordModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationRecord<QuestionDiscussionModel>> GetOutboxMessagesAsync(int pageIndex, int pageSize, int userId)
        {
            try
            {
                PaginationRecord<QuestionDiscussion> questionDiscussion = await _unitOfWork.GetRepository<QuestionDiscussion>().GetAllAsync(pageIndex, pageSize, x => x.Id, new Expression<Func<QuestionDiscussion, bool>>[] { x => x.RepliedId == userId }, OrderBy.Descending);

                PaginationRecord<QuestionDiscussionModel> paginationRecordModel = new PaginationRecord<QuestionDiscussionModel>
                {
                    DataRecord = _imapper.Map<IEnumerable<QuestionDiscussionModel>>(questionDiscussion.DataRecord),
                    CountRecord = questionDiscussion.CountRecord,
                };

                return paginationRecordModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationRecord<QuestionDiscussionModel>> GetUnReadMessagesAsync(int pageIndex, int pageSize, int userId)
        {
            try
            {
                PaginationRecord<QuestionDiscussion> questionDiscussion = await _unitOfWork.GetRepository<QuestionDiscussion>().GetAllAsync(pageIndex, pageSize, x => x.Id, new Expression<Func<QuestionDiscussion, bool>>[] { x => x.RepliedId == userId, y => y.IsRead == false }, OrderBy.Descending);

                PaginationRecord<QuestionDiscussionModel> paginationRecordModel = new PaginationRecord<QuestionDiscussionModel>
                {
                    DataRecord = _imapper.Map<IEnumerable<QuestionDiscussionModel>>(questionDiscussion.DataRecord),
                    CountRecord = questionDiscussion.CountRecord,
                };

                return paginationRecordModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<QuestionDiscussionModel>> GetAllConversationAsync(int senderId, int receiverId, int QuestionId)
        {
            try
            {
                return await _unitOfWork.QuestionDiscussionRepository.GetAllConversationAsync(senderId, receiverId, QuestionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<List<QuestionDiscussionModel>> GetAllConversationAsyncList(int senderId, int receiverId, int QuestionId)
        //{
        //    try
        //    {
        //        return await _unitOfWork.QuestionDiscussionRepository.GetAllConversationAsyncList(senderId, receiverId, QuestionId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public QuestionDiscussionModel AddQuestionDiscussion(QuestionDiscussionModel model)
        {
            try
            {
                QuestionDiscussion questionDiscussion = _imapper.Map<QuestionDiscussion>(model);
                _unitOfWork.GetRepository<QuestionDiscussion>().Insert(questionDiscussion);

                if (_unitOfWork.SaveChanges())
                {

                    User user = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == questionDiscussion.CreatedBy);

                    UserRole roles = _dbFactory.GetDataContext.UserRoles.FirstOrDefault(s => s.UserId == questionDiscussion.CreatedBy);

                    //var  rolesForUser =  _userManager.GetRolesAsync(user);
                    // //if (rolesForUser[0] != "Guest")
                    // //{
                    // //}

                    if (roles.RoleId == 5)
                    {
                        _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                        {
                            SenderId = questionDiscussion.CreatedBy,
                            RecipientId = questionDiscussion.RepliedId,
                            MessageText = "NewMessageWasAdded",
                            IsRead = false,
                            Deleted = false,
                            NotificationTypeId = (int)SystemEnum.NotificationType.Chatting,
                            ReferenceMassageId = questionDiscussion.Id,
                        });
                    }

                }

                QuestionDiscussionModel questionDiscussionModel = _imapper.Map<QuestionDiscussionModel>(questionDiscussion);
                questionDiscussionModel.SenderId = questionDiscussion.CreatedBy;
                return questionDiscussionModel;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        public bool PublishQuestiondiscussion(int questionId)
        {
            try
            {
                bool status = false;
                bool found = false;
                QuestionDiscussion question = null;
                List<QuestionDiscussion> consultationDiscussions = _unitOfWork.GetRepository<QuestionDiscussion>().GetWhere(x => x.QuestionId == questionId && x.IsRead == false).ToList();
                //  consultationDiscussions.ToList();
                foreach (QuestionDiscussion p in consultationDiscussions)
                {
                    p.IsPublished = true;
                    _unitOfWork.GetRepository<QuestionDiscussion>().Update(p);
                    found = true;
                }
                if (found)
                {
                    question = consultationDiscussions.Last();
                }
                if (_unitOfWork.SaveChanges())
                {
                    status = true;
                    if (question != null)
                    {
                        _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                        {
                            SenderId = question.CreatedBy,
                            RecipientId = question.RepliedId,
                            MessageText = "NewMessageWasAdded",
                            IsRead = false,
                            Deleted = false,
                            NotificationTypeId = (int)SystemEnum.NotificationType.Chatting,
                            ReferenceMassageId = question.Id,
                        });
                    }

                }

                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteQuestionDiscussion(int id)
        {
            try
            {
                QuestionDiscussion questionDiscussion = _unitOfWork.GetRepository<QuestionDiscussion>().GetSingle(id);
                _unitOfWork.GetRepository<QuestionDiscussion>().Delete(questionDiscussion);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<int> GetUnReadMessages(int userId)
        {
            try
            {
                return await _unitOfWork.QuestionDiscussionRepository.GetUnReadMessages(userId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> MarkMessageAsRead(int userId, int messageId)
        {
            try
            {
                QuestionDiscussion questionDiscussion = await _unitOfWork.GetRepository<QuestionDiscussion>().GetSingleAsync(messageId);

                if (questionDiscussion.RepliedId == userId)
                {
                    questionDiscussion.IsRead = true;
                    questionDiscussion.DateRead = DateTime.Now;
                }

                _unitOfWork.GetRepository<QuestionDiscussion>().Update(questionDiscussion);
                return _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
