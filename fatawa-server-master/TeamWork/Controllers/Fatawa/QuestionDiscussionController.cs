using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.Fatawa
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionDiscussionController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public QuestionDiscussionController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("QuestionDiscussion")]
        public async Task<IActionResult> QuestionDiscussion(int id)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionDiscussionService.GetQuestionDiscussion(id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut]
        [Route("PublishQuestiondiscussion")]
        public IActionResult PublishQuestiondiscussion(int questionId)
        {
            try
            {
                return Ok(_unitOfWorkService.QuestionDiscussionService.PublishQuestiondiscussion(questionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("AddQuestionDiscussion")]
        public IActionResult AddQuestionDiscussion(QuestionDiscussionModel questionDiscussionModel)
        {
            try
            {
                return Ok(_unitOfWorkService.QuestionDiscussionService.AddQuestionDiscussion(questionDiscussionModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllConversationAsync")]
        public async Task<IActionResult> GetAllConversationAsync(int receiverId, int questionId)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionDiscussionService.GetAllConversationAsync(CurrentUserId, receiverId, questionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetUnReadMessages")]
        public async Task<IActionResult> GetUnReadMessages()
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionDiscussionService.GetUnReadMessages(CurrentUserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("MarkMessageAsRead")]
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionDiscussionService.MarkMessageAsRead(CurrentUserId, messageId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
