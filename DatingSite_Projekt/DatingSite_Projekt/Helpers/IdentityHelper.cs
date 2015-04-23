using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Dating_data.Repository;

namespace DatingSite_Projekt.Helpers
{
    public class IdentityHelper
    {

        public static int CurrentUserId()
        {
            var id = HttpContext.Current.User.Identity.Name;
            int userId;
            int.TryParse(id, out userId);
            return userId;

        }

    }
}