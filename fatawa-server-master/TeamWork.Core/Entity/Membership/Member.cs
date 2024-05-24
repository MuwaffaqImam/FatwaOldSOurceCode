using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Membership
{
    public class Member : BaseEntity
    {
        [Column(TypeName = "nvarchar(250)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string SecondName { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string LastName { get; set; }

        public int? ImageId { get; set; }

        public int MembershipTypeId { get; set; }
        [ForeignKey("MembershipTypeId")]
        public virtual MembershipType MembershipType { get; set; }


        //ToDo : should be spertated table for the Gender
        public int Gender { get; set; }
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
