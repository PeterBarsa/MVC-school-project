using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatingSite_Projekt.Models
{
    public class FriendRequestModel
    {
        public int senderId { get; set; }
        public int userId { get; set; }
        public string senderName { get; set; }

    }
}