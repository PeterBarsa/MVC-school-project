using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;
using DatingSite_Projekt.Models;
using Dating_data.Repository;

namespace DatingSite_Projekt.Api
{
    public class MessageController : ApiController
    {
        [HttpPost]
        public void PostMessageDetails(MessageModel message)
        {

            MessageRepository.InsertMessage(message.Message, message.ReceiverId, message.SenderId);

        }

    }
}
