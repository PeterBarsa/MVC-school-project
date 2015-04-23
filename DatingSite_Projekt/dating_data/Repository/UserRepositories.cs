using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Dating_data.Repository
{
    public class UserRepositories
    {

        //skapar en metod för att hämta alla tillgängliga användare i databasen
        public static List<User> GetUsers()
        {
            using (var context = new MainDBContext())
            {

                //skapar en lista sorterad enligt id'na av databasens Users tabell
                var result = context.Users
                   
                    .OrderBy(x => x.Id)
                    .ToList();

                //returnerar listan
                return result;

            }
        }

        //metod som returnerar en användares användarnamn baserat på id't som skickas med i parametern
        public static string GetUserName(int id)
        {
            using (var context = new MainDBContext())
            {
                //returnerar användarnamnet där id't passar in.
                return context.Users.Where(x => x.Id == id).Select(x => x.Username).FirstOrDefault();
            }
        }

        //en metod som returnerar en användare där användarnamnet och lösenordet överrensstämmer varandra
        public static User TestLogIn(string username, string password)
        {
            using (var context = new MainDBContext())
            {
                return context.Users.FirstOrDefault(x=> x.Username.Equals(username) && x.Password.Equals(password));
            }
        }

        //en metod som hämtar en specifik användares lösenord, skickar med en användares id som parameter
        public static string GetPasswordForUser(int userId)
        {
            using (var context = new MainDBContext())
            {

                // använder id't för att hämta ut användaren där id't matchar.
                var user = context.Users.FirstOrDefault(x => x.Id == userId);

                //returnerar användarens lösenord.
                return user.Password;
            }
        }

        //metod som sparar alla förändringar gjorda för en användare, tar emot lösenord, användarnamn och id som parameter.
        public static void SetUserChanges(string password, string username, int userId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en variabel av användaren där id't passar in
                var user = context.Users.FirstOrDefault(x => x.Id == userId);

                //fyller i den nya informationen
                user.Password = password;
                user.Username = username;

                //sparar förändringarna
                context.SaveChanges();
            }
        }

        //samma som ovan, men används om användaren endast byter sitt användarnamn och inte användarnamn och lösenord.
        public static void SetUserChanges(string username, int userId)
        {
            using (var context = new MainDBContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                user.Password = username;
                context.SaveChanges();
            }
        }

        //metod som bestämmer om användaren ska vara sökbar eller inte ta emot en boolean searchable och användarens id
        public static void SetUserSearchable(bool searchable, int userId)
        {
            using (var context = new MainDBContext())
            {
                //hämtar ut användaren vars id är medskickat och sätter värdet searchable till parametern searchable's värde och sparar
                //sedan förändringarna.
                var user = context.Users.FirstOrDefault(x => x.Id == userId);
                user.Searchable = searchable;
                context.SaveChanges();
            }
        }

        //metod för att lägga till en ny användare, nästan all information som ska införas skickas med i parametrarna.
        public static void AddNewUser(string username, string password, string city, string country, string email, int age,int id)
        {

            using (var context = new MainDBContext())
            {

                //skapar två variabler, en som ska införas i tabellen Users och en i Descriptions
                var user = new User();
                var description = new Description();
                //Lägger till informationen för Users tabellen i variabeln user som skapades och lägger till den
                //i databasen(inga förändringar sparade än)
                user.Username = username;
                user.Password = password;
                //skapar ett id som aldrig kommer krocka med en tidigare användare
                user.Id = GetUsers().Count + 1;
                user.Searchable = true;
                context.Users.Add(user);

                //Lägger till informationen för Descriptions tabellen i variabeln description som skapades 
                //och lägger till den i databasen(inga förändringar sparade än) samt ett autoId skapas för beskrivningen
                description.DescriptionId = DescriptionRepository.GetDescriptions().Count + 1;
                description.UserId = user.Id;
                description.City = city;
                description.Country = country;
                description.Email = email;
                description.Age = age;
                context.Descriptions.Add(description);

                //förändringarna i databasen sparas
                context.SaveChanges();
            }
        }
    }
}
