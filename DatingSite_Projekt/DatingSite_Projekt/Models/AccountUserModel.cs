using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Web;

namespace DatingSite_Projekt.Models
{
    public class AccountUserModel
    {
        [Display(Name = "Description: ")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [DataType(DataType.Text, ErrorMessage = "")]
        public string Description { get; set; }

        [DataType(DataType.Text, ErrorMessage = "enter a username")]
        [Display(Name = "Username: ")]
        public string Username { get; set; }

        [Display(Name = "Searchable: ")]
        public bool Searchable { get; set; }

        [DataType(DataType.Text, ErrorMessage = "")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Display(Name = "About Me: ")]
        public string AboutMe { get; set; }

        [Display(Name = "Email: ")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Enter a correct email address!")]
        [DataType(DataType.Text, ErrorMessage = "Enter an email")]
        [Required(ErrorMessage = "Cannot be empty!")]
        public string Email { get; set; }

        [RegularExpression(@"^(0?[1-9]|[1-9][0-9])$", ErrorMessage = "Only numbers 0-99 ")]
        [Display(Name = "Age: ")]
        public int Age { get; set; }

        [DataType(DataType.Text, ErrorMessage = "")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Display(Name = "Country: ")]
        public string Country { get; set; }

        [DataType(DataType.Text, ErrorMessage = "")]
        [Required(ErrorMessage = "Cannot be empty!")]
        [Display(Name = "City: ")]
        public string City { get; set; }

        public int? UserId { get; set; }
    }
}