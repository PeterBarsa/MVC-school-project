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

        public static Description GetDescription(int? userId)
        {
            using (var context = new MainDBContext())
            {
                //hämtar ut description för användaren där id passar in som användarid't
                return context.Descriptions.FirstOrDefault(x => x.UserId == userId);
                
            }
        }

        public static void InsertNewDescription(Description d)
        {
            using (var context = new MainDBContext())
            {
               
                var desc = context.Descriptions.FirstOrDefault(x => x.UserId == d.UserId);
                desc.Description1 = d.Description1;
                desc.AboutMe = d.AboutMe;
                desc.Country = d.Country;
                desc.City = d.City;
                desc.Email = d.Email;
                desc.Age = d.Age;
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
