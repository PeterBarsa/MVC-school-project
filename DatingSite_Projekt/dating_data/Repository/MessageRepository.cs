using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dating_data.Repository
{
    public class MessageRepository
    {

        //metod för att returnera en användares meddelanden, tar emot en nullable int som parameter som tillhör personen vars
        //meddelanden man hämtar ut
        public static List<Message> GetMessageList(int? id)
        {
            using (var context = new MainDBContext())
            {
                //skapar en lista av typen Message från databasen som hämtar alla meddelanden i databasen och ordnar dem i listan
                //från högsta id't på meddelandet till lägsta (ordnar listan i senaste meddelandet först i vyn senare)
                var messageList = new List<Message>(context.Messages.Select(x => x).OrderByDescending(x=>x.MessageId));

                //skapar en resultatlista a typen Message
                var result = new List<Message>();

                //loopar igenom listan
                foreach (var m in messageList)
                {

                    //ifall meddelandets mottagarid matchar id't som mottogs i parametern så läggs meddelandet till i resultatlistan.
                    if (m.ReceiverId == id)
                    {
                        result.Add(m);
                    }
                }

                //returnerar resultatet.
                return result;

            }
        }

        //metod för att föra in ett nytt meddelande, tar emot meddelandet, mottagarid't och avsändarid't
        public static void InsertMessage(string message, int rId, int sId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en variabel av typen Message från databasen och fyller informationen från parametrarna i rätt plats
                var dbmessage = new Message();
                dbmessage.Messages = message;
                dbmessage.SenderId = sId;
                dbmessage.ReceiverId = rId;

                //slaåar en lista av alla id'n i Messages, räknar upp dem och lägger till +1 och skapar således hela tiden ett nytt 
                //meddelande id.
                var idLista = context.Messages.Select(x => x.ReceiverId).ToList();
                dbmessage.MessageId = idLista.Count() + 1;

                //lägger till meddelandet i databasen
                context.Messages.Add(dbmessage);

                //sparar förändringarna i databasen
                context.SaveChanges();


            }
        }

    }
}