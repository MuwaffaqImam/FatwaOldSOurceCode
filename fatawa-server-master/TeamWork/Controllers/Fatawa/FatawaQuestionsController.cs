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
    public class FatawaQuestionsController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public FatawaQuestionsController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("GetAllQuestionsAsync")]
        public async Task<IActionResult> GetAllQuestionsAsync(int pageIndex = 1, int pageSize = 10, int questionStateId = 1, int MuftiId = -1)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionService.GetAllQuestionsAsync(pageIndex, pageSize, questionStateId, MuftiId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AddQuestion")]
        public IActionResult AddQuestion(QuestionModel questionModel)
        {
            try
            {
                return Ok(_unitOfWorkService.QuestionService.AddQuestion(questionModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getAllQuestionsByStatusId")]
        public async Task<IActionResult> getAllQuestionsByStatusId(int pageIndex = 1, int pageSize = 10, int statusId = 0, int MuftiId = 0)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionService.getAllQuestionsByStatusId(pageIndex, pageSize, statusId, CurrentUserId, MuftiId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateCurrentStatusQuestion")]
        public IActionResult UpdateCurrentStatusQuestion(int questionId, int statueId)
        {
            try
            {
                return Ok(_unitOfWorkService.QuestionService.UpdateCurrentStatusQuestion(questionId, statueId, CurrentUserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetQuestionById")]
        public async Task<IActionResult> GetQuestionById(int questionId)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionService.GetQuestionById(questionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateCloseQuestion")]
        public IActionResult UpdateCloseQuestion(int questionId)
        {
            try
            {
                return Ok(_unitOfWorkService.QuestionService.UpdateCloseQuestion(questionId, CurrentUserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetUserIdAddedQuestion")]
        public async Task<IActionResult> GetUserIdAddedQuestion(int questionId)
        {
            try
            {
                return Ok(await _unitOfWorkService.QuestionService.GetUserIdAddedQuestion(questionId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
