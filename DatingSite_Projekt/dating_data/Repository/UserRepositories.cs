using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Dating_data.Repository
{
    public class UserRepositories
    {

        public static List<User> GetUsers()
        {
            using (var context = new MainDBContext())
            {

                var result = context.Users.OrderBy(x => x.Id).ToList();

                return result;

            }
        }

        private static User GetUser(int? userId)
        {
            using (var context = new MainDBContext())
            {
                return context.Users.FirstOrDefault(x => x.Id == userId);
            }
        }

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

        public static string GetPasswordForUser(int userId)
        {
            using (var context = new MainDBContext())
            {

                var user = GetUser(userId);

                return user.Password;
            }
        }

        public static void SetUserChanges(User u)
        {
            using (var context = new MainDBContext())
            {

                var user = GetUser(u.Id);
                user.Password = u.Password;
                user.Username = u.Username;

                context.SaveChanges();
            }
        }

        //samma som ovan, men används om användaren endast byter sitt användarnamn och inte användarnamn och lösenord.
        public static void SetUserChanges(string username, int userId)
        {
            using (var context = new MainDBContext())
            {
                var user = GetUser(userId);
                user.Password = username;
                context.SaveChanges();
            }
        }

        //metod som bestämmer om användaren ska vara sökbar eller inte ta emot en boolean searchable och användarens id
        public static void SetUserSearchable(bool searchable, int userId)
        {
            using (var context = new MainDBContext())
            {
                var user = GetUser(userId);
                user.Searchable = searchable;
                context.SaveChanges();
            }
        }

        public static void AddNewUser(string username, string password, string city, string country, string email, int age,int id)
        {

            using (var context = new MainDBContext())
            {

                //skapar två objekt, en som ska införas i tabellen Users och en i Descriptions
                
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Searchable = true
                };


                var description = new Description
                {
                    UserId = user.Id,
                    City = city,
                    Country = country,
                    Email = email,
                    Age = age
                };

                context.Users.Add(user);
                context.Descriptions.Add(description);

                context.SaveChanges();
            }
        }
    }
}
