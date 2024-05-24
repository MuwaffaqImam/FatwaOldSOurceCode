using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository.ICustomRepsitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TeamWork.Core.Entity.Security;
using Microsoft.AspNetCore.Identity;

namespace TeamWork.Repository.Repository.CustomRepository
{
    internal class QuestionDiscussionRepository : Repository<QuestionDiscussion>, IQuestionDiscussionRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        internal QuestionDiscussionRepository(IDbFactory dbFactory, IMapper mapper, UserManager<User> userManager) : base(dbFactory)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IEnumerable<QuestionDiscussionModel>> GetAllConversationAsync(int senderId, int receiverId, int QuestionId)
        {
            try
            {

                User user = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == senderId);
               // User user1 = _dbFactory.GetDataContext.Users.FirstOrDefault(s => s.Id == receiverId);
                var rolesForUser = await _userManager.GetRolesAsync(user);
                IEnumerable<QuestionDiscussionModel> questionDiscussionModel;

                if (rolesForUser[0] != "Guest")
                {
                    questionDiscussionModel = await (from qd in _dbFactory.GetDataContext.QuestionDiscussion
                                                     join q in _dbFactory.GetDataContext.Question on qd.QuestionId equals q.Id
                                                     join userSender in _dbFactory.GetDataContext.Users on qd.CreatedBy equals userSender.Id
                                                     join userRecevier in _dbFactory.GetDataContext.Users on qd.CreatedBy equals userRecevier.Id
                                                     where qd.QuestionId == QuestionId
                                                     select new QuestionDiscussionModel
                                                     {
                                                         Id = qd.Id,
                                                         CreatedBy = qd.CreatedBy,
                                                         CreatedDate = qd.CreatedDate,
                                                         UpdatedBy = qd.UpdatedBy,
                                                         UpdatedDate = qd.UpdatedDate,
                                                         MessageText = qd.MessageText,
                                                         IsRead = qd.IsRead,
                                                         DateRead = qd.DateRead,
                                                         QuestionId = qd.QuestionId,
                                                         SenderId = qd.CreatedBy,
                                                         SenderName = userSender.UserName,
                                                         SenderPhotoUrl = userSender.PhotoURL,
                                                         RepliedId = qd.RepliedId,
                                                         RepliedName = userRecevier.UserName,
                                                         RepliedPhotoUrl = userRecevier.PhotoURL,
                                                         IsPublished = qd.IsPublished

                                                     }
                                                                        ).OrderByDescending(o => o.CreatedDate).ToListAsync();
                }
                else
                {

                    questionDiscussionModel = await (from qd in _dbFactory.GetDataContext.QuestionDiscussion
                                                     join q in _dbFactory.GetDataContext.Question on qd.QuestionId equals q.Id
                                                     join userSender in _dbFactory.GetDataContext.Users on qd.CreatedBy equals userSender.Id
                                                     join userRecevier in _dbFactory.GetDataContext.Users on qd.CreatedBy equals userRecevier.Id
                                                     where (qd.RepliedId == senderId && qd.CreatedBy == receiverId ||
                                                           qd.RepliedId == receiverId && qd.CreatedBy == senderId)
                                                           && (qd.IsPublished == true)
                                                     select new QuestionDiscussionModel
                                                     {
                                                         Id = qd.Id,
                                                         CreatedBy = qd.CreatedBy,
                                                         CreatedDate = qd.CreatedDate,
                                                         UpdatedBy = qd.UpdatedBy,
                                                         UpdatedDate = qd.UpdatedDate,
                                                         MessageText = qd.MessageText,
                                                         IsRead = qd.IsRead,
                                                         DateRead = qd.DateRead,
                                                         QuestionId = qd.QuestionId,
                                                         SenderId = qd.CreatedBy,
                                                         SenderName = userSender.UserName,
                                                         SenderPhotoUrl = userSender.PhotoURL,
                                                         RepliedId = qd.RepliedId,
                                                         RepliedName = userRecevier.UserName,
                                                         RepliedPhotoUrl = userRecevier.PhotoURL,
                                                         IsPublished = qd.IsPublished

                                                     }
                                                                      ).OrderByDescending(o => o.CreatedDate).ToListAsync();
                }





                return questionDiscussionModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetUnReadMessages(int userId)
        {
            try
            {
                IEnumerable<QuestionDiscussion> questionDiscussion = await _dbFactory.GetDataContext.QuestionDiscussion.Where(m => m.IsRead == false && m.RepliedId == userId).ToListAsync();
                return questionDiscussion.Count();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
