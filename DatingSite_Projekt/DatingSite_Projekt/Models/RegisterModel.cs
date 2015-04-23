using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DatingSite_Projekt.Models
{
    public class RegisterModel
    {

        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Display(Name = "Username: ")]
        [DataType(DataType.Text, ErrorMessage = "Enter a username")]
        [Required(ErrorMessage = "Cannot be empty!")]
        public string Username { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Enter a password")]
        [Display(Name = "Password: ")]
        [Required(ErrorMessage = "Password is required!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{3,16}$", ErrorMessage = "Password must be between 3 and 16 characters long and contain at least one big letter one small letter and a number!")]
        public string Password { get; set; }

        [Display(Name = "Country: ")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Text, ErrorMessage = "Enter a country")]
        public string Country { get; set; }

        [Display(Name = "City: ")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Text, ErrorMessage = "Enter a city")]
        public string City { get; set; }

        [Display(Name="Email: ")]
        [DataType(DataType.Text, ErrorMessage = "enter an email")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Enter a correct email address!")]
        public string Email { get; set; }

        [RegularExpression(@"^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Only numbers 0-99 ")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Display(Name="Age: ")]
        public int Age { get; set; }


    }
}
