using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Dating_data.Repository
{
    public class FriendsRepository
    {

        //metod för att hämta vänförfrågningarna för nuvarande inloggad användare, tar emot användarens id som parameter
        public static List<Friend> GetFriendRequest(int userId)
        {
            using (var context = new MainDBContext())
            {
                // hämtar vännerlistan för användaridt som skickats med och lagrar dem i en lista av typen Friend som är
                //hämtad från databasen
                List<Friend> friendRequestList = new List<Friend>(context.Friends.Where(x => x.User2 == userId).Select(x => x));

                //skapar en till lista av typen Friend som kommer bli slutgiltiga resultatet som returneras vidare.
                List<Friend> result = new List<Friend>();

                //loopar igenom listan på alla vänner som vart hämtade ur databasen
                foreach (var f in friendRequestList)
                {

                    //ifall status på vännen INTE är true, dvs. det är fortfarande en förfrågan så läggs objektet in i resultat listan
                    if (!f.status == true)
                    {
                        result.Add(f);
                    }
                }

                //returnerar resultatet
                return result;
            }


        }

        //Hämtar vännerlistan för nuvarande användare, tar emot användarens id som parameter
        public static List<User> GetFriends(int userId)
        {
            using (var context = new MainDBContext())
            {

                //skapar tre listor, två av typen Friend som hämtar vänner informationen i korsad ordning, dvs att listan hämtar 
                //alla förekommande kombinationer av användarens id, och kan därför se sin vän oavsett vem som skicka förfrågan
                //(förs in i databasen i specifik ordning), den tredje listan är av typen int och kommer lagra alla
                //vänners id som hämtas ut från dessa två listor
                List<Friend> friendList1 = new List<Friend>(context.Friends.Where(x => x.User2 == userId).Select(x => x));
                List<Friend> friendList2 = new List<Friend>(context.Friends.Where(x => x.User1 == userId).Select(x => x));
                List<int> idListComplete = new List<int>();
                
                //lägger till alla vänner i friendList1 som inte har status false, dvs. är en vän och inte en vänförfrågan
                foreach (var f in friendList1)
                {
                    if (!f.status == false)
                    {
                        idListComplete.Add(f.User1);
                    }


                }

                //lägger till alla vänner i friendList2 som inte har status false
                foreach (var f in friendList2)
                {
                    if (!f.status == false)
                    {
                        idListComplete.Add(f.User2);
                    }


                }

                //skapar en lista av typen User vid namn result som skall returneras och fylls på med alla användare
                //vars id finns med i idListComplete
                var result = new List<User>();
                foreach (var id in idListComplete)
                {
                    var user = context.Users.FirstOrDefault(x => x.Id == id);
                    result.Add(user);
                }

                //returnerar listan result
                return result;
            }
        }

        //metod för att lägga till en ny vänförfrågning, tar emot förfrågarens id och mottagarens id som parametrar
        public static void NewFriendRequest(int senderId, int receiverId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en lista av alla vänner/vänförfrågningar i databasen, en status som används för en if sats som är förinställd på 1
                //och en variabel vid namn request av typen Friend från databasens tabell.
                var currentRequestList = context.Friends.Select(x => x).ToList();
                var status = 1;
                var request = new Friend();
                //fyller i värdena i förfrågninen, status förinställd som false då det är en förfrågan, samt att alla nuvarande inlägg
                // i vännerlistan räknas upp och lägger till +1 för att sätta id't på vänförfrågan automatiskt till något som inte existerar.
                request.User1 = senderId;
                request.User2 = receiverId;
                request.status = false;
                request.FriendId = currentRequestList.Count() + 1;

                //loopar igenom listan med nuvarande vänner/förfrågningar
                foreach (var r in currentRequestList)
                {
                    //skapar en variabel för den nuvarande aktiva vännen/förfrågningen och fyller i idn'a med nuvarande värden
                    var current = new Friend();
                    current.User1 = r.User1;
                    current.User2 = r.User2;

                    //kollar i båda korshåll ifall förfrågan redan finns, dvs id'na kan existera på både user1 och user 2 och
                    //blir då upptäckta, ett felmeddelande skickas och variabeln status sätts till 0 och loopen stannar
                    if (current.User1 == request.User1 && current.User2 == request.User2)
                    {
                        MessageBox.Show("The request already exists!");
                        status = 0;
                        break;
                    }
                    if (current.User2 == request.User1 && current.User1 == request.User2)
                    {
                        MessageBox.Show("The request already exists!");
                        status = 0;
                        break;
                    }
                }
                //ifall förfrågan inte stoppades i loopen kommer status förbli 1 och vänförfrågan läggs in i databasen och sparas.
                if (status == 1)
                {
                    context.Friends.Add(request);
                    context.SaveChanges();
                }

            }
        }

        //metod för att acceptera vänförfrågan tar emot ett id för den som skicka förfrågan och ett för den som mottar förfrågan.
        public static void AcceptFriend(int senderId, int receiverId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en ny variabel av typen Friend från databasen och tilldelar den informationen från databasen som
                //passar in där id't för den som skicka förfrågan och mottagarens id passar in korrekt.
                var friend = new Friend();
                friend = context.Friends.FirstOrDefault(x => x.User1 == senderId && x.User2 == receiverId);
                //ändrar statusen för förfrågan till true och sparar förändringarna
                friend.status = true;
                context.SaveChanges();
            }
        }

        //metod för att se om en vänförfrågan till specifik användare existerar eller ej
        public static bool GetFriendReqStatus(int userId)
        {

            using (var context = new MainDBContext())
            {
                // hämtar en lista av alla vänförfrågningar som finns för en särskild användare (status false = förfrågan, en true = vänner)
                var friendRequestList = context.Friends.Select(x => x).Where(x => x.User2 == userId && x.status == false);
                //returnerar true ifall det finns en förfrågan i listan, false om inte
                return friendRequestList.Any();

                
            }

        }

    }
}
