using System;

namespace TeamWork.DTO.Security
{
    public class UserResetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
