using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatingSite_Projekt.Models
{
    public class MessageModel
    {
        public string Message { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
    }
}