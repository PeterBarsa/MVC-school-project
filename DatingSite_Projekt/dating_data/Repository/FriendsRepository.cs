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
        public static List<Friend> GetFriendRequest(int userId)
        {
            using (var context = new MainDBContext())
            {
                var friendRequestList = new List<Friend>(context.Friends.Where(x => x.User2 == userId).Select(x => x));
                List<Friend> result = new List<Friend>();

                foreach (var f in friendRequestList)
                {

                    //ifall status på vännen INTE är true, dvs. det är fortfarande en förfrågan så läggs objektet in i resultat listan
                    if (!f.status == true)
                    {
                        result.Add(f);
                    }
                }

                return result;
            }


        }

        public static List<User> GetFriends(int userId)
        {
            using (var context = new MainDBContext())
            {

                //skapar tre listor, två av typen Friend som hämtar vänner informationen i korsad ordning, dvs att listan hämtar 
                //alla förekommande kombinationer av användarens id, och kan därför se sin vän oavsett vem som skicka förfrågan
                //(förs in i databasen i specifik ordning), den tredje listan är av typen int och kommer lagra alla
                //vänners id som hämtas ut från dessa två listor
                var friendList1 = new List<Friend>(context.Friends.Where(x => x.User2 == userId).Select(x => x));
                var friendList2 = new List<Friend>(context.Friends.Where(x => x.User1 == userId).Select(x => x));
                var idListComplete = new List<int>();
                
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

                return result;
            }
        }


        public static void NewFriendRequest(int senderId, int receiverId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en lista av alla vänner/vänförfrågningar i databasen, en status som används för en if sats som är förinställd på 1
                //och en variabel vid namn request av typen Friend från databasens tabell.
                var currentRequestList = context.Friends.Select(x => x).ToList();
                var status = 1;
                var request = new Friend
                {
                    User1 = senderId,
                    User2 = receiverId,
                    status = false
                };

                //loopar igenom listan med nuvarande vänner/förfrågningar
                foreach (var r in currentRequestList)
                {
                    var current = new Friend
                    {
                        User1 = r.User1, User2 = r.User2
                    };

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

                if (status == 1)
                {
                    context.Friends.Add(request);
                    context.SaveChanges();
                }

            }
        }

        public static void AcceptFriend(int senderId, int receiverId)
        {
            using (var context = new MainDBContext())
            {


                var friend = context.Friends.FirstOrDefault(x => x.User1 == senderId && x.User2 == receiverId);
                //ändrar statusen för förfrågan till true och sparar förändringarna
                friend.status = true;
                context.SaveChanges();
            }
        }

        public static bool GetFriendReqStatus(int userId)
        {

            using (var context = new MainDBContext())
            {
                
                var friendRequestList = context.Friends.Select(x => x).Where(x => x.User2 == userId && x.status == false);
                return friendRequestList.Any();

                
            }

        }

    }
}
