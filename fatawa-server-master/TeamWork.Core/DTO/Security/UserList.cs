using System;

namespace TeamWork.DTO.Security
{
    public class UserList
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime CreateDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
    }
}
