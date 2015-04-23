using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Dating_data.Repository
{
    public class DescriptionRepository
    {
        //metod för att hämta beskrivning på en särskildanvändare, tar emot en nullable int som parameter som matchar
        //användarens id som man vill hämta beskrivningen åt.
        public static Description GetDescription(int? userId)
        {
            using (var context = new MainDBContext())
            {
                //hämtar ut description för användaren där id passar in som användarid't
                return context.Descriptions.FirstOrDefault(x => x.UserId == userId);
                
            }
        }

        //metod för att införa en ny beskrivning för en användare, tar emt alla värden var för sig i parametrar
        public static void InsertNewDescription(string description, string aboutMe, string country, string city, string email, int? age, int userId)
        {
            using (var context = new MainDBContext())
            {
                //skapar en variabel för en användares description där id't som skickats med som parameter matchar in i databasen.
                //fyller sedan i all information man fått från parametrarna där dem ska sitta i tabellen för användaren
                Description desc = context.Descriptions.FirstOrDefault(x => x.UserId == userId);
                desc.Description1 = description;
                desc.AboutMe = aboutMe;
                desc.Country = country;
                desc.City = city;
                desc.Email = email;
                desc.Age = age;

                //sparar sedan förändringarna i databasen.
                context.SaveChanges();
            }
        }

        //en metod som används för att lägga till nya descriptions i en annan metod(UserRepositories.AddNewUser) och hämtar därför en lista på alla descriptions Id'n
        //och returnerar listan.
        public static List<int> GetDescriptions()
        {
            using (var context = new MainDBContext())
            {

                var result = context.Descriptions.Select(x => x.DescriptionId).ToList();

                return result;

            }
        }

    }
}
