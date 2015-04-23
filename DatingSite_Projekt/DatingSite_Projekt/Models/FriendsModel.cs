using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatingSite_Projekt.Models
{
    public class FriendsModel
    {

        
        public int Id { get; set; }
        public string Username { get; set; }


        public FriendsModel(int id, string username)
        {
            this.Id = id;
            this.Username = username;

        }
    }
}