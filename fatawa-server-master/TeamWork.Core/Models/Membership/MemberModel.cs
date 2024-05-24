using System;

namespace TeamWork.Core.Models.Membership
{
    public class MemberModel : BaseModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public int? ImageId { get; set; }

        public int MembershipTypeId { get; set; }
        public string MembershipTypeName { get; set; }


        //ToDo : should be spertated table for the Gender
        public int Gender { get; set; }
        public int GenderName { get; set; }

        public DateTime BirthDate { get; set; }

        //ToDo : should be spertated table for the Countries
        public string Nationality { get; set; }

        //ToDo : should be spertated table for the Qualification
        public string Qualification { get; set; }

        public string Certificates { get; set; }
        public string ExternalCourses { get; set; }
        public string Researchs { get; set; }
    }
}
