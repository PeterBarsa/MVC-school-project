using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;
using DatingSite_Projekt.Resources;

namespace DatingSite_Projekt.Models
{
    public class HomeLoginModel
    {


    
    [DataType(DataType.Text, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "userRequired",
    ErrorMessageResourceType = typeof(login))]
    public string Username { get; set; }
    
    [DataType(DataType.Password, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "passRequired",
    ErrorMessageResourceType = typeof(login))]
    public string Password { get; set; }

    }
}