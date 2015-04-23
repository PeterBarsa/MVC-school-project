using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dating_data.Repository;

namespace DatingSite_Projekt.Api.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDto(User u)
        {
            this.Id = u.Id;
            this.Username = u.Username;
            this.Password = u.Password;
        }

    }

}