using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dating_data.Repository
{
    public class MessageRepository
    {


        public static List<Message> GetMessageList(int? id)
        {
            using (var context = new MainDBContext())
            {

                var messageList = new List<Message>(context.Messages.Select(x => x).OrderByDescending(x=>x.MessageId));

                var result = new List<Message>();
                foreach (var m in messageList)
                {

                    //ifall meddelandets mottagarid matchar id't som mottogs i parametern så läggs meddelandet till i resultatlistan.
                    if (m.ReceiverId == id)
                    {
                        result.Add(m);
                    }
                }

                return result;

            }
        }

        public static void InsertMessage(string message, int rId, int sId)
        {
            using (var context = new MainDBContext())
            {
                var dbmessage = new Message
                {
                    Messages = message,
                    SenderId = sId,
                    ReceiverId = rId
                };

                context.Messages.Add(dbmessage);
                context.SaveChanges();


            }
        }

    }
}