using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NextekkManagmt.ViewModels
{

    public class UserEmployeeViewModel
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public int NumberOfChildren { get; set; }

        [DataType(DataType.Currency)]
        public decimal AnnualSalary { get; set; }

        public Guid GenderID { get; set; }

        public Guid EducationQualificationID { get; set; }

        public Guid MaritalStatusID { get; set; }

        public string NameOfInstitution { get; set; }

        public string Position { get; set; }

        public string FullName
        {
            get
            {
                return LastName + ' ' + FirstName;
            }
        }


        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [MinLength(6, ErrorMessage = "Password length should be at least 6 characters")]
        [Required]
        public string Password { get; set; }


        [Compare("Password")]
        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters")]
        public string ConfirmPassword { get; set; }
    }
}