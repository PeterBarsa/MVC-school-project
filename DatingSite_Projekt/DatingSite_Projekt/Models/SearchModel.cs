using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using Dating_data.Repository;

namespace DatingSite_Projekt.Models
{
    public class SearchModel
    {

        public string Username { get; set; }
        public int UserId { get; set; }
        public bool Search { get; set; }

        public SearchModel(User u)
        {
            this.Username = u.Username;
            this.UserId = u.Id;
            this.Search = u.Searchable;
        }

        public SearchModel()
        {

        }
    }
}