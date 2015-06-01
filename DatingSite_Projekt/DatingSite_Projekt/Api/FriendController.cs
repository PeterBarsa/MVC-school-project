using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Forms;
using DatingSite_Projekt.Helpers;
using DatingSite_Projekt.Models;
using Dating_data.Repository;

namespace DatingSite_Projekt.Api
{
    public class FriendController : ApiController
    {


        [HttpPost]
        public void PostFriendRequest(FriendRequestModel friend)
        {

            var receiverId = friend.userId;
            var senderId = friend.senderId;
            FriendsRepository.NewFriendRequest(senderId, receiverId);

        }

        public void AcceptFriendRequest(FriendRequestModel friend)
        {
            var receiverId = IdentityHelper.CurrentUserId();
            FriendsRepository.AcceptFriend(friend.senderId, receiverId);
        }

        public void DenyFriendRequest(FriendRequestModel friend)
        {
            var receiverId = IdentityHelper.CurrentUserId();
            
        }

    }
}
