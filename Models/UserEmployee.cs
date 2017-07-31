using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NextekkManagmt.Models
{
    public class UserEmployee
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public Guid GenderID { get; set; }

        public Guid MaritalStatusID { get; set; }

        public int NumberOfChildren { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfEmployment { get; set; }

        public Guid EducationQualificationID { get; set; }

        public string NameOfInstitution { get; set; }

        public bool Active { get; set; }
    
        public string Position { get; set; }

        public string Email { get; set; }

        public string RegistrationID { get; set; }

        [DataType(DataType.Date)]
        public DateTime PromotedAt { get; set; }

        [DataType(DataType.Currency)]
        public decimal AnnualSalary { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual MaritalStatus MaritalStatus { get; set; }

        public virtual EducationQualification EducationQualification { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ' ' + FirstName;
            }
        }


    }
}