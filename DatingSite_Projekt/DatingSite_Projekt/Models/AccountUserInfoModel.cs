using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatingSite_Projekt.Models
{
    public class AccountUserInfoModel
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Enter your old password")]
        [Display(Name = "Enter your old password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Enter a password")]
        [Display(Name = "Insert your new password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{3,16}$", ErrorMessage = "Password must be between 3 and 16 characters long and contain at least one big letter one small letter and a number!")]
        public string NewPassword { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Something went wrong, only use letters or numbers.")]
        [Display(Name = "Insert your new Username")]
        public string NewUsername { get; set; }

    }
}