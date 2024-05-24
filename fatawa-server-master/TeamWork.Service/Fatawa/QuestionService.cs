using AutoMapper;
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
using static TeamWork.Core.DTO.SystemEnum;
using Microsoft.EntityFrameworkCore;

namespace TeamWork.Service.Fatawa
{
    internal class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;
        private readonly IDbFactory _dbFactory;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;


        internal QuestionService(IUnitOfWork unitOfWork, IMapper imapper, IDbFactory dbFactory, Microsoft.Extensions.Configuration.IConfiguration iConfig)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
            _dbFactory = dbFactory;
            _configuration = iConfig;

        }

        public async Task<PaginationRecord<QuestionModel>> GetAllQuestionsAsync(int pageIndex, int pageSize, int questionStateId, int MuftiId)
        {
            try
            {

                int MuftiUserId = Convert.ToInt32(_configuration.GetSection("MySettings").GetSection("MuftiUserId").Value);
                int levels = 1;
                PaginationRecord<Question> questions;
                UserTree MuftiUser = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.mufitUserId == MuftiId);

                if (MuftiUser != null)
                {
                    if (MuftiUser.ParentId != MuftiUserId)
                    {
                        UserTree MuftiUser1 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser.ParentId);
                        if (MuftiUser1 != null)
                        {
                            if (MuftiUser1.ParentId != MuftiUserId)
                            {
                                UserTree MuftiUser2 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser1.ParentId);
                                if (MuftiUser2 != null)
                                {
                                    if (MuftiUser1.ParentId != MuftiUserId)
                                    {
                                        levels = 3;
                                        MuftiId = MuftiUser2.mufitUserId;
                                    }
                                }

                            }
                            else
                            {
                                levels = 2;
                                MuftiId = MuftiUser1.mufitUserId;
                            }
                        }
                        else
                        {

                            MuftiId = 1;
                        }
                    }
                }

                if (levels == 3)
                {
                    questionStateId = 4;
                }

                if (levels == 1 || levels == 2)
                {

                    questions = await _unitOfWork.GetRepository<Question>().GetAllAsync(pageIndex, pageSize, x => x.Id,
                        new Expression<Func<Question, bool>>[] { x => x.StatusId == questionStateId, x => x.TransferUserId == -1 },
                                                                 OrderBy.Descending);
                }
                else
                {

                    questions = await _unitOfWork.GetRepository<Question>().GetAllAsync(pageIndex, pageSize, x => x.Id,
                     new Expression<Func<Question, bool>>[] { x => x.StatusId == questionStateId, x => x.TransferUserId == MuftiId },
                                                              OrderBy.Descending);
                }


                //PaginationRecord<QuestionModel> paginationRecordModel = new PaginationRecord<QuestionModel>
                //{
                //    DataRecord = _imapper.Map<IEnumerable<QuestionModel>>(questions.DataRecord),
                //    CountRecord = questions.CountRecord,
                //};

                //questions = await _unitOfWork.GetRepository<Question>().GetAllAsync(pageIndex, pageSize, x => x.Id, predicators.ToArray(), OrderBy.Ascending);
                var questionTotalCount = questions.DataRecord.Count();

                PaginationRecord<QuestionModel> paginationRecordModel = new PaginationRecord<QuestionModel>
                {
                    DataRecord = questions.DataRecord.Select(x => new QuestionModel
                    {
                        Id = x.Id,
                        FatawaQuestion = x.FatawaQuestion,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        StatusId = x.StatusId,
                        StatusUserId = x.StatusUserId,
                        askedFullName = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == x.CreatedBy).FullName,

                    }),

                    CountRecord = questionTotalCount
                };

                return paginationRecordModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginationRecord<QuestionModel>> getAllQuestionsByStatusId(int pageIndex, int pageSize, int statusId, int userId, int MuftiId)
        {
            try
            {
                PaginationRecord<Question> questions = new PaginationRecord<Question>();



                List<Expression<Func<Question, bool>>> predicators = new List<Expression<Func<Question, bool>>>();
                QuestionStatus questionStatusName = ((QuestionStatus)statusId);
                List<int> userValidationStatus = new List<int> { (int)QuestionStatus.InProgress, (int)QuestionStatus.Rejected, (int)QuestionStatus.Closed };
                UserTree MuftiUser = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.mufitUserId == MuftiId);
                int MuftiUserId = Convert.ToInt32(_configuration.GetSection("MySettings").GetSection("MuftiUserId").Value);
                int levels = 1;
                if (MuftiUser != null)
                {
                    if (MuftiUser.ParentId != MuftiUserId)
                    {
                        UserTree MuftiUser1 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser.ParentId);
                        if (MuftiUser1 != null)
                        {
                            if (MuftiUser1.ParentId != MuftiUserId)
                            {
                                UserTree MuftiUser2 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser1.ParentId);
                                if (MuftiUser2 != null)
                                {
                                    if (MuftiUser1.ParentId != MuftiUserId)
                                    {
                                        levels = 3;
                                        MuftiId = MuftiUser2.mufitUserId;
                                    }
                                }

                            }
                            else
                            {
                                levels = 2;
                                MuftiId = MuftiUser1.mufitUserId;
                            }
                        }
                        else
                        {

                            MuftiId = 1;
                        }
                    }
                }




                if (levels == 3)
                {
                    statusId = 4;
                }

                if (userValidationStatus.Contains(statusId) && statusId != 2)
                {
                    predicators.Add(questionStatus => questionStatus.StatusUserId == userId);
                }
                predicators.Add(questionStatus => questionStatus.StatusId == statusId);
                if (levels == 1 || levels == 2)
                {
                    if (statusId == 1)
                    {
                        predicators.Add(questionStatus => questionStatus.TransferUserId == -1 | questionStatus.TransferUserId == MuftiId);
                        //predicators.Add(questionStatus => );
                    }
                    else
                        predicators.Add(questionStatus => questionStatus.TransferUserId == MuftiId);

                }

                else
                    predicators.Add(questionStatus => questionStatus.TransferUserId == MuftiId);


                questions = await _unitOfWork.GetRepository<Question>().GetAllAsync(pageIndex, pageSize, x => x.Id, predicators.ToArray(), OrderBy.Ascending);
                var questionTotalCount = questions.DataRecord.Count();

                PaginationRecord<QuestionModel> paginationRecordModel = new PaginationRecord<QuestionModel>
                {
                    DataRecord = questions.DataRecord.Select(x => new QuestionModel
                    {
                        Id = x.Id,
                        FatawaQuestion = x.FatawaQuestion,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate,
                        StatusId = x.StatusId,
                        StatusUserId = x.StatusUserId,
                        askedFullName = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == x.CreatedBy).FullName,

                    }),

                    CountRecord = questionTotalCount
                };


                //PaginationRecord<QuestionModel> paginationRecordModel = new PaginationR ecord<QuestionModel>
                //{
                //    DataRecord = _imapper.Map<IEnumerable<QuestionModel>>(questions.DataRecord),
                //    CountRecord = questions.CountRecord,
                //};

                return paginationRecordModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<QuestionModel> GetAllQuestions(string tags)
        {
            try
            {
                IEnumerable<Question> questions = _unitOfWork.GetRepository<Question>().GetAll();
                return _imapper.Map<List<QuestionModel>>(questions);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public QuestionModel GetQuestion(int id)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(id);
                return _imapper.Map<QuestionModel>(question);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddQuestion(QuestionModel model)
        {
            try
            {
                Question question = _imapper.Map<Question>(model);
                question.TransferUserId = -1;
                _unitOfWork.GetRepository<Question>().Insert(question);

                if (_unitOfWork.SaveChanges())
                {
                    //the sender is the logged in user and RecipientId should be based on the business 
                    _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                    {
                        SenderId = question.CreatedBy,
                        MessageText = "NewQuestionWasAdded",
                        IsRead = false,
                        Deleted = false,
                        NotificationTypeId = (int)SystemEnum.NotificationType.Question,
                        RecipientRoleId = (int)SystemEnum.RolesType.Admin
                    });
                }

                return question.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateQuestion(QuestionModel model)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(model.Id);
                _unitOfWork.GetRepository<Question>().Update(question);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteQuestion(int id)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(id);
                _unitOfWork.GetRepository<Question>().Delete(question);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void ChangeLastSeen(int id)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(id);
                _unitOfWork.GetRepository<Question>().Update(question);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void IncreaseVistorsCount(int id)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(id);
                _unitOfWork.GetRepository<Question>().Update(question);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateCurrentStatusQuestion(int questionId, int statusId, int statusUserId)
        {
            try
            {
                Question question = _unitOfWork.GetRepository<Question>().GetSingle(questionId);
                QuestionStatus questionStatusName = ((QuestionStatus)statusId);
                UserTree MuftiUser = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.mufitUserId == statusUserId);
                int MuftiUserId = Convert.ToInt32(_configuration.GetSection("MySettings").GetSection("MuftiUserId").Value);
                int MuftiId = -1;
                if (MuftiUser != null)
                {
                    if (MuftiUser.ParentId != MuftiUserId)
                    {
                        UserTree MuftiUser1 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser.ParentId);
                        if (MuftiUser1 != null)
                        {
                            if (MuftiUser1.ParentId != MuftiUserId)
                            {
                                UserTree MuftiUser2 = _dbFactory.GetDataContext.UserTree.FirstOrDefault(s => s.Id == MuftiUser1.ParentId);
                                if (MuftiUser2 != null)
                                {
                                    if (MuftiUser1.ParentId != MuftiUserId)
                                    {
                                        MuftiId = MuftiUser2.mufitUserId;
                                    }
                                }

                            }
                            else
                            {
                                MuftiId = MuftiUser1.mufitUserId;
                            }
                        }
                        else
                        {

                            MuftiId = 1;
                        }
                    }
                    else
                    {
                        MuftiId = statusUserId;
                    }
                }

                if (question != null)
                {

                    question.StatusId = (int)questionStatusName;
                    question.StatusUserId = statusUserId;
                    question.TransferUserId = MuftiId;
                }

                _unitOfWork.GetRepository<Question>().Update(question);
                return _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<QuestionModel> GetQuestionById(int questionId)
        {
            try
            {
                return await _unitOfWork.QuestionRepository.GetQuestionById(questionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCloseQuestion(int questionId, int statusUserId)
        {
            try
            {
                return await _unitOfWork.QuestionRepository.UpdateCloseQuestion(questionId, statusUserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetUserIdAddedQuestion(int questionId)
        {
            QuestionModel questionModel = new QuestionModel();
            try
            {
                Question question = await _unitOfWork.GetRepository<Question>().GetSingleAsync(questionId);

                if (questionModel != null)
                {
                    questionModel = _imapper.Map<QuestionModel>(question);
                }
                return questionModel.CreatedBy;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
