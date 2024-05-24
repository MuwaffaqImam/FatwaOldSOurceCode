using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Services;

namespace TeamWork.Controllers.Notification
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;

        public NotificationController(IUnitOfWorkService unitOfWorkService)
        {
            _unitOfWorkService = unitOfWorkService;
        }

        [HttpGet]
        [Route("GetUnreadNotification")]
        public async Task<IActionResult> GetUnreadNotification()
        {
            try
            {
                return Ok(await _unitOfWorkService.NotificationService.GetUnreadNotification(CurrentUserId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateReadNewNotification")]
        public IActionResult UpdateReadNewNotification(NotificationItemModel notificationItemModel)
        {
            try
            {
                return Ok(_unitOfWorkService.NotificationService.UpdateReadNewNotification(CurrentUserId, notificationItemModel.CreatedBy, notificationItemModel.NotificationTypeId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetNewQuestionsAndFatwa")]
        public async Task<IActionResult> GetNewQuestionsAndFatwa()
        {
            try
            {
                return Ok(await _unitOfWorkService.NotificationService.GetNewQuestionsAndFatwa(CurrentUserRole));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("UpdateReadNewQuestionsAndFatwa")]
        public IActionResult UpdateReadNewQuestionsAndFatwa(NotificationItemModel notificationItemModel)
        {
            try
            {
                return Ok(_unitOfWorkService.NotificationService.UpdateReadNewQuestionsAndFatwa(CurrentUserRole, notificationItemModel.NotificationTypeId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
