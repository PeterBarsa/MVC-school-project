using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Forms;
using DatingSite_Projekt.Helpers;
using DatingSite_Projekt.Models;
using Dating_data.Repository;

namespace DatingSite_Projekt.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        
        public ActionResult Profile()
        {

            //fyller i informationen i en model för den nuvarande inloggade användern med hjälp av namnet på kakan
            var model = new AccountUserModel();
            model.Username = UserRepositories.GetUserName(IdentityHelper.CurrentUserId());
            var results = DescriptionRepository.GetDescription(IdentityHelper.CurrentUserId());
            int age = results.Age ?? default(int);
            model.Description = results.Description1;
            model.AboutMe = results.AboutMe;
            model.Age = age; 
            model.City = results.City;
            model.Country = results.Country;
            model.Email = results.Email;

            var messageList = new List<MessageModel>();
            var tempListMessage = MessageRepository.GetMessageList(IdentityHelper.CurrentUserId());
            foreach (var m in tempListMessage)
            {
                var message = new MessageModel { Message = m.Messages, SenderName = UserRepositories.GetUserName(m.SenderId), SenderId = m.SenderId };
                messageList.Add(message);
            }

            //skickar med listan, modellen för meddelanden och id för nuvarande inloggad person till vyn
            ViewData["Message"] = messageList;
            ViewData["MessageModel"] = new MessageModel();
            ViewData["senderId"] = IdentityHelper.CurrentUserId();
            return View(model);
        }
        
        public ActionResult Friends()
        {

            var friendList = new List<FriendsModel>();
            var friends = FriendsRepository.GetFriends(IdentityHelper.CurrentUserId());
            foreach (var friend in friends)
            {
                var newFriend = new FriendsModel(friend.Id, friend.Username);
                friendList.Add(newFriend);
            }

            return View(friendList);
        }

        
        public ActionResult FriendRequest()
        {
            
            var friendRequests = FriendsRepository.GetFriendRequest(IdentityHelper.CurrentUserId());
            List<FriendRequestModel> friendRequestModels = new List<FriendRequestModel>();
            foreach (var f in friendRequests)
            {
                var friend = new FriendRequestModel();
                friend.senderId = f.User1;
                friend.userId = IdentityHelper.CurrentUserId();
                friend.senderName = UserRepositories.GetUserName(friend.senderId);
                friendRequestModels.Add(friend);
                
            }
            
            return View(friendRequestModels);
        }

      
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        //skapar en funktion för att ladda upp en fil från vyn Edit, tar emot filen som postats från hemsidan som parameter
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {

            if (file != null)
            {

                var extension = Path.GetExtension(file.FileName);
                //slår ihop serverns väg där filen skapas och döper om filen till användarnamnet + tidigare extension
                string path = Path.Combine(
                              Server.MapPath("~/Images/Profile"), UserRepositories.GetUserName(IdentityHelper.CurrentUserId()) + extension);


                var allowedExtensions = new[] { ".jpg", ".JPG", ".jpeg" };
                if (!allowedExtensions.Contains(extension))
                {
                    
                    TempData["failed/succeded"] = "Upload failed! Only .jpg filetypes allowed!";
                    RedirectToAction("Edit", "Account", ModelState);
                }
                else
                { 
                //om allt gått bra så sparas filen lokalt med variabeln path som sökväg
                TempData["failed/succeded"] = "Upload was successful";
                file.SaveAs(path);
                }
            }


            return RedirectToAction("Edit", "Account");
        
        }


        public ActionResult Edit()
        {
            
            var model = new AccountUserModel();
            var results = DescriptionRepository.GetDescription(IdentityHelper.CurrentUserId());
            int age = results.Age ?? default(int);
            model.Description = results.Description1;
            model.AboutMe = results.AboutMe;
            model.Age = age; 
            model.City = results.City;
            model.Country = results.Country;
            model.Email = results.Email;

            //ifall en TempData skickats med från uppladdningsmetoden skickas den vidare till vyn med en ViewData av typen string
            if (TempData["failed/succeded"] != null)
            {
                ViewData["UploadMessage"] = TempData["failed/succeded"] as string;
            }
            
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(AccountUserModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            
            DescriptionRepository.InsertNewDescription(model.Description, model.AboutMe, model.Country, model.City, model.Email, model.Age, IdentityHelper.CurrentUserId());
            UserRepositories.SetUserSearchable(model.Searchable, IdentityHelper.CurrentUserId());
            ViewData["Success"] = "Changes to your profile has been made!";
            return View();

        }


        public ActionResult EditUserInfo()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult EditUserInfo(AccountUserInfoModel model)
        {
            //hämtar id't på nuvarande användare samt lösenord och en lista av alla användare
            var userId = IdentityHelper.CurrentUserId();
            var currentUserPassword = UserRepositories.GetPasswordForUser(userId);
            var comparison = UserRepositories.GetUsers();
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            //Kontrollerar så det gamla lösenordet överrensstämmer med det som finns lagrat i databasen för användaren.
            if (!model.OldPassword.Equals(currentUserPassword))
            {
                ModelState.AddModelError("", "Wrong password!");
                return View(model);
            }

            foreach (var user in comparison)
            {

                if (user.Username.Equals(model.NewUsername) && !model.NewUsername.Equals(UserRepositories.GetUserName(userId)))
                {
                    ModelState.AddModelError("", "Username already exists!");
                    return View(model);
                }

            }

            //Ifall inget nytt användarnamn är ifyllt fylls ditt gamla in.
            if (model.NewUsername == null)
            {
                model.NewUsername = UserRepositories.GetUserName(userId);
            }

            //Ifall du inte har infört några nya detaljer till ditt användarnamn eller lösenord så kommer ett meddelande fram.
            else if (model.NewUsername.Equals(UserRepositories.GetUserName(userId)) && model.NewPassword == null)
            {
                ModelState.AddModelError("", "You've made no changes to your account!");
                return View(model);
            }

            //Ifall du endast byter användarnamn och inte tillför något nytt lösenord byts ditt användarnamn.
            else if (!model.NewUsername.Equals(UserRepositories.GetUserName(userId)) && model.NewPassword == null)
            {
                ModelState.AddModelError("", "Username is updated!");
                UserRepositories.SetUserChanges(model.NewUsername, userId);
                return View(model);
            }

          
            ModelState.AddModelError("", "Username and Password or just password has been updated!");
            UserRepositories.SetUserChanges(model.NewPassword, model.NewUsername, userId);
            return View(model);
        }


        public ActionResult Details(int id)
        {

            //ifall medskickade id't är den nuvarande användarens id skickas man till sin egen profil.
            if(id == IdentityHelper.CurrentUserId())
            {
                return RedirectToAction("Profile", "Account");
            }

            int? userId = id;
            var usersPage = new AccountUserModel();
            var results = DescriptionRepository.GetDescription(userId);

                int age = results.Age ?? default(int);
                usersPage.Description = results.Description1;
                usersPage.AboutMe = results.AboutMe;
                usersPage.Age = age;
                usersPage.City = results.City;
                usersPage.Country = results.Country;
                usersPage.Email = results.Email;
                usersPage.Username = UserRepositories.GetUserName(id);


                //skapar en lista av MessageModel och fyller den med alla meddelanden som tillhör id't som skickats med
                var messageList = new List<MessageModel>();
                var tempListMessage = MessageRepository.GetMessageList(userId);
                foreach (var m in tempListMessage)
                {
                    var message = new MessageModel { Message = m.Messages, SenderName = UserRepositories.GetUserName(m.SenderId), SenderId = m.SenderId };
                    messageList.Add(message);
                }

                //skickar med listan, nuvarande användares id och id't vars vy du ska se i ViewData som hämtas i vyn
                //används för att lista meddelanden och sedan för att skicka dem. Du returneras sedan till vyn Details med modellen för 
                //användaren
                ViewData["Message"] = messageList;
                ViewData["senderId"] = IdentityHelper.CurrentUserId();
                ViewData["receiverId"] = userId;

                return View(usersPage);
           
        }


        public ActionResult SearchUsers()
        {
            return View();
        }

      
        [HttpPost]
        public ActionResult SearchUsers(SearchModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();

            }

            var result = new List<SearchModel>();
            var users = UserRepositories.GetUsers();

            //ifall man sökt med en tom sträng så förändras värdet från nul till "" för att göra en sökning på alla namn i databasen
            if (model.Username == null)
            {
                model.Username = "";
            }


            foreach (var u in users)
            {

                if (u.Username.StartsWith(model.Username) && u.Searchable == true)
                {

                    var user = new SearchModel(u);
                    result.Add(user);
                }
            }

            //resultatet skickas med som en TempData till vyn SearchUserResult och skickar vidare användaren dit.
            TempData["resultData"] = result;
            return RedirectToAction("SearchUserResult", "Account");
        }
       

        public ActionResult SearchUserResult()
        {
            //hämtar ut TempDatan som skickades med från ovanståend ActionResult och skickar med listan till vyn
            var results = TempData["resultData"] as List<SearchModel>;
            return View(results);
        }

        

    }
}