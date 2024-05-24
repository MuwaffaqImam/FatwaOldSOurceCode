using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using TeamWork.Controllers.Base;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Models.Security;
using TeamWork.Core.Services;
using TeamWork.DTO.Security;

namespace TeamWork.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : TeamControllerBase
    {
        private readonly IUnitOfWorkService _unitOfWorkService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public AuthenticationController(IUnitOfWorkService unitOfWorkService, IConfiguration configuration, IMapper mapper, ILogger<AuthenticationController> logger)
        {
            _unitOfWorkService = unitOfWorkService;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("UserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            try
            {
                var loginResult = await _unitOfWorkService.SecurityService.Login(userLogin);

                if (loginResult != null)
                {
                    return Ok(loginResult);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("SocialLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> SocialLogin(SocialUserModel socialUserModel)
        {
            try
            {
                var loginResult = await _unitOfWorkService.SecurityService.SocialLogin(socialUserModel);

                if (loginResult != null)
                {
                    return Ok(loginResult);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                User user = await _unitOfWorkService.SecurityService.FindByEmailAsync(email);

                if (user == null)
                {
                    throw new Exception("User Not Exist!");
                }
                IdentityResult result = await _unitOfWorkService.SecurityService.ConfirmEmailAsync(user, token);
                string userLanguage = user.LanguageId == 1 ? "Arabic" : "English";
                string viewPage = string.Format(result.Succeeded ? "../../Views/{0}/EmailConfirmed" : "../../Views/{0}/Error", userLanguage);
                ViewData["WebSiteURL"] = _configuration.GetSection("AllowedClient").Value;
                return View(viewPage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(int fromadmin,[FromBody] UserRegister userRegister)
        {
            try
            {
                User userToCreate = _mapper.Map<User>(userRegister);
                userToCreate.IsActive = true;
                userToCreate.CreateDate = DateTime.Now;
                userToCreate.SecurityStamp = Guid.NewGuid().ToString();// Add this to avoid error when update roles
                userToCreate.UserName = userRegister.Email;
                userRegister.UserName = userRegister.Email;
                if (fromadmin ==1)
                {
                   
                    userRegister.EmailConfirmed = true;
                    userRegister.IsActive = true;
                    
                }
                

                IdentityResult result = await _unitOfWorkService.SecurityService.CreateUserAsync(userRegister);
                _logger.LogInformation("result-succ");
                if (result.Succeeded)
                {
                    UserRegister userToReturn = _mapper.Map<UserRegister>(userToCreate);
                    User userManager = await _unitOfWorkService.SecurityService.FindByNameAsync(userToCreate.UserName);
                    string token = await _unitOfWorkService.SecurityService.GenerateEmailConfirmationTokenAsync(userToReturn);
                    string confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = userToCreate.Email }, Request.Scheme);
                    string emailSubject = userToCreate.LanguageId == 1 ? "تأكيد البريد الإلكتروني" : "Confirmation email";
                    string userLanguage = userToCreate.LanguageId == 1 ? "Arabic" : "English";
                    string viewPath = Path.Combine(string.Format("Views/{0}", userLanguage), "ConfirmEmail.html");
                    string template = System.IO.File.ReadAllText(viewPath);
                    template = template.Replace("CONFIRMATION_URL", confirmationLink);
                    await _unitOfWorkService.SecurityService.SendEmailAsync(new string[] { userToCreate.Email }, emailSubject, template);
                    await _unitOfWorkService.SecurityService.AddToRoleAsync(userToReturn, "guest");
                    return Ok(userToReturn);
                }
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("IsUserExists")]
        [AllowAnonymous]
        public async Task<bool> IsUserExists(string userEmail)
        {
            try
            {
                var userManager = await _unitOfWorkService.SecurityService.FindByEmailAsync(userEmail);
                return (userManager != null) ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetAllUsersAsync")]
        public async Task<IActionResult> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return Ok(new
                {
                    data = await _unitOfWorkService.SecurityService.GetAllUsersAsync(pageNumber, pageSize),
                    total = _unitOfWorkService.SecurityService.GetUsersCountRecord()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("SearchByUserName")]
        public async Task<IActionResult> SearchByUserName(string searchUserName)
        {
            try
            {
                return Ok(await _unitOfWorkService.SecurityService.GetUserBySearchNameAsync(searchUserName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CompleteResetPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteResetPassword([FromForm] UserResetPassword userResetPassword)
        {
            try
            {
                if (userResetPassword.Password != userResetPassword.ConfirmPassword ||
                    string.IsNullOrEmpty(userResetPassword.Password) ||
                    string.IsNullOrEmpty(userResetPassword.Email) ||
                    string.IsNullOrEmpty(userResetPassword.Token))
                {
                    throw new Exception("Invalid reset password request!");
                }
                User user = await _unitOfWorkService.SecurityService.FindByEmailAsync(userResetPassword.Email);

                if (user == null)
                {
                    throw new Exception("User Not Exist!");
                }
                string userLanguage = user.LanguageId == 1 ? "Arabic" : "English";
                IdentityResult result = await _unitOfWorkService.SecurityService.ResetPasswordAsync(user, userResetPassword.Token, userResetPassword.Password);
                string viewPage = string.Format(result.Succeeded ? "../../Views/{0}/ResetPasswordDone" : "../../Views/{0}/Error", userLanguage);

                ViewData["WebSiteURL"] = string.Format("{0}/auth/login", _configuration.GetSection("AllowedClient").Value);
                return View(viewPage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("viewForResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewForResetPassword(string email, string token)
        {
            try
            {
                User user = await _unitOfWorkService.SecurityService.FindByEmailAsync(email);

                if (user == null)
                {
                    throw new Exception("User Not Exist!");
                }
                string userLanguage = user.LanguageId == 1 ? "Arabic" : "English";
                string viewPage = string.Format("../../Views/{0}/ResetPassword", userLanguage);
                UserResetPassword userResetPassword = new UserResetPassword();
                userResetPassword.Email = email;
                userResetPassword.Token = token;
                return View(viewPage, userResetPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(UserResetPassword userResetPassword)
        {
            try
            {
                User user = await _unitOfWorkService.SecurityService.FindByEmailAsync(userResetPassword.Email);
                string token = await _unitOfWorkService.SecurityService.GeneratePasswordResetTokenAsync(user);
                string confirmationLink = Url.Action(nameof(ViewForResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                string emailSubject = user.LanguageId == 1 ? "إعادة ضبط كلمة المرور" : "Reset Password";
                string userLanguage = user.LanguageId == 1 ? "Arabic" : "English";
                string viewPath = Path.Combine(string.Format("Views/{0}", userLanguage), "ConfirmEmailForResetPassword.html");
                string template = System.IO.File.ReadAllText(viewPath);
                template = template.Replace("CONFIRMATION_URL", confirmationLink);
                await _unitOfWorkService.SecurityService.SendEmailAsync(new string[] { user.Email }, emailSubject, template);

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
