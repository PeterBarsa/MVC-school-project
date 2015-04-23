using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dating_data.Repository;

namespace DatingSite_Projekt.Helpers
{
    public class FriendRequestHelper
    {
        public static bool FriendRequestStatus()
        {

            return FriendsRepository.GetFriendReqStatus(IdentityHelper.CurrentUserId());
        }
    }
}