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
    //säger att användaren måste vara igenkännbar för att komma åt denna klass
    [Authorize]
    public class AccountController : Controller
    {
        
        //hämtar en vy för Profile
        public ActionResult Profile()
        {

            //skapar en modell av typen AccountUserModel
            var model = new AccountUserModel();
            //fyller i informationen i en model för den nuvarande inloggade användern med hjälp av namnet på kakan
            model.Username = UserRepositories.GetUserName(IdentityHelper.CurrentUserId());
            var results = DescriptionRepository.GetDescription(IdentityHelper.CurrentUserId());
            //konverterar en nullable int från resultatet till en vanlig int så den kan användas i modellen
            int age = results.Age ?? default(int);
            //överför informationen hämtad ovan från variabeln results och för över till modellen
            model.Description = results.Description1;
            model.AboutMe = results.AboutMe;
            model.Age = age; 
            model.City = results.City;
            model.Country = results.Country;
            model.Email = results.Email;
            //Hämtar meddelandena för användaren som är inloggad
            var messageList = new List<MessageModel>();
            var tempListMessage = MessageRepository.GetMessageList(IdentityHelper.CurrentUserId());
            foreach (var m in tempListMessage)
            {
                //skapar en ny modell av alla Messages som hämtas och för in i listan för att sedan returneras tillsidan
                var message = new MessageModel { Message = m.Messages, SenderName = UserRepositories.GetUserName(m.SenderId), SenderId = m.SenderId };
                messageList.Add(message);
            }

            //skickar med listan, modellen för meddelanden och id för nuvarande inloggad person till vyn
            ViewData["Message"] = messageList;
            ViewData["MessageModel"] = new MessageModel();
            ViewData["senderId"] = IdentityHelper.CurrentUserId();
            //skickar med modellen in i vyn
            return View(model);
        }
        
        //hämtar en vy för Friends
        public ActionResult Friends()
        {
            //skapar en lista av typen FriendsModel
            var friendList = new List<FriendsModel>();
            //hämtar alla vänner för nuvarande inloggad användare. och skickar in den till listan vi skapa ovan.
            var friends = FriendsRepository.GetFriends(IdentityHelper.CurrentUserId());
            foreach (var friend in friends)
            {
                var newFriend = new FriendsModel(friend.Id, friend.Username);
                friendList.Add(newFriend);
            }
            //skickar med listan in i vyn
            return View(friendList);
        }

        //skapar en vy för FriendRequest
        public ActionResult FriendRequest()
        {

            //hämtar alla vänförfrågningar för nuvarande inloggad användare och för in dem i en lista av typen FriendRequestModel
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
            //skickar med listan in i vyn
            return View(friendRequestModels);
        }

       //skapar en funktion för att logga ut
        public ActionResult LogOut()
        {
            //tar bort kakan och skicka användaren till Login vyn i HomeController
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        //skapar en funktion för att ladda upp en fil från vyn Edit, tar emot filen som postats från hemsidan som parameter
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            //kollar så filens värde inte är null
            if (file != null)
            {
                //slår ihop serverns väg där filen skapas och döper om filen till användarnamnet.jpg
                string path = Path.Combine(
                              Server.MapPath("~/Images/Profile"), UserRepositories.GetUserName(IdentityHelper.CurrentUserId()) + ".jpg");
                //skapar en array som innehåller två strängar, strängarna används för att verifiera att endast.jpg bilder är tillåtna
                var allowedExtensions = new[] { ".jpg", ".JPG" };
                var extension = Path.GetExtension(file.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    //skapar ett felmeddelande som skickas till Edit kontrollern
                    TempData["failed/succeded"] = "Upload failed! Only .jpg filetypes allowed!";
                    RedirectToAction("Edit", "Account", ModelState);
                }
                else
                { 
                    //om allt gått bra så sparas filen lokalt med variabeln path som sökväg
                file.SaveAs(path);
                }
            }
            // efter uppladdning så skickas du till Edit kontrollern med ett meddelande
            TempData["failed/succeded"] = "Upload was successful";
            return RedirectToAction("Edit", "Account");
        
        }

       //skapar Edit vyn
        public ActionResult Edit()
        {
            //skapar en modell för typen AccountUserModel
            var model = new AccountUserModel();
            //Hämtar information från nuvarande inloggad användare
            var results = DescriptionRepository.GetDescription(IdentityHelper.CurrentUserId());
            //konverterar en nullable int från resultatet till en vanlig int så den kan användas i modellen
            int age = results.Age ?? default(int);
            //fyller modellen med den hämtade informationen
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
            //returnerar vyn med en medskickad modell
            return View(model);
        }

        //hanterar när information från vyn sparas, använder en AccountUserModel som parameter att fylla information i
        [HttpPost]
        public ActionResult Edit(AccountUserModel model)
        {
            //kollar så att alla fält är ifyllda korrekt enligt modellens annotations, skickas tillbaka till vyn med felmeddelande annars
            if (!ModelState.IsValid)
            {
                return View();
            }
            //ifall allt går igenom skickas den sparade informationen till datalagret för att föras in i tabellerna och du returneras
            //sedan tillbaka till vyn med ett meddelande att förändringarna har lyckats
            DescriptionRepository.InsertNewDescription(model.Description, model.AboutMe, model.Country, model.City, model.Email, model.Age, IdentityHelper.CurrentUserId());
            UserRepositories.SetUserSearchable(model.Searchable, IdentityHelper.CurrentUserId());
            ViewData["Success"] = "Changes to your profile has been made!";
            return View();

        }

        //skapar en vy för EditUserInfo som används för att ändra lösenord och användarnamn
        public ActionResult EditUserInfo()
        {
            return View();
        }

        //använder en AccountUserInfoModel för att fylla i korrekt information i vyn och skickas sedan till denna metod som parameter
        [HttpPost]
        public ActionResult EditUserInfo(AccountUserInfoModel model)
        {
            //hämtar id't på nuvarande användare samt lösenord och en lista av alla användare
            var userId = IdentityHelper.CurrentUserId();
            var currentUserPassword = UserRepositories.GetPasswordForUser(userId);
            var comparison = UserRepositories.GetUsers();
            //kollar så alla annotations stämmer och returnerar i annat fall en vy med felmeddelanden
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
                //kollar så användarnamnet inte redan existerar och jämför inte med användarens nuvarande användarnamn.
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
            if (model.NewUsername.Equals(UserRepositories.GetUserName(userId)) && model.NewPassword == null)
            {
                ModelState.AddModelError("", "You've made no changes to your account!");
                return View(model);
            }
            //Ifall du endast byter användarnamn och inte tillför något nytt lösenord byts ditt användarnamn.
            if (!model.NewUsername.Equals(UserRepositories.GetUserName(userId)) && model.NewPassword == null)
            {
                ModelState.AddModelError("", "Username is updated!");
                UserRepositories.SetUserChanges(model.NewUsername, userId);
                return View(model);
            }

            //ifall allt har gått vägen så skickas informationen till datalagret för att sparas och användaren
            //skickas tillbaka med vyn med ett meddelande om att allt gått bra
            ModelState.AddModelError("", "Username and Password or just password has been updated!");
            UserRepositories.SetUserChanges(model.NewPassword, model.NewUsername, userId);
            return View(model);
        }

        //skapar Details vyn, avsedd för andra användares/vänners profiler
        //tar emot id't på användaren vars vy du vill se
        public ActionResult Details(int id)
        {

            //ifall medskickade id't är den nuvarande användarens id skickas man till sin egen profil.
            if(id == IdentityHelper.CurrentUserId())
            {
                return RedirectToAction("Profile", "Account");
            }
            //omvandlar id't till en nullable int för att datalagret ska kunna matcha det mot tabellen
            //skapar en modell av AccountUserModel vid namn usersPage och hämtar informationen från descriptions tabellen för användaren
            //vars vy du ska se
            int? userId = id;
            var usersPage = new AccountUserModel();
            var results = DescriptionRepository.GetDescription(userId);
            //kollar så att resultatet inte är null och kör isådannafall nedanstående kod som fyller modellen med resultatets information
                //konverterar en nullable int från resultatet till en vanlig int så den kan användas i modellen
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

        // returnerar vyn för SearchUsers
        public ActionResult SearchUsers()
        {
            return View();
        }

        //hanterar informationen som skickats med från vyn ovan
        [HttpPost]
        public ActionResult SearchUsers(SearchModel model)
        {
            //kollar så modellen är korrekt skickar annars tillbaka till vyn
            if (!ModelState.IsValid)
            {
                return View();

            }
            //skapar en lista vid namn resultat och hämtar sedan alla användare i databasen
            var result = new List<SearchModel>();
            var users = UserRepositories.GetUsers();

            //ifall man sökt med en tom sträng så förändras värdet från nul till "" för att göra en sökning på alla namn i databasen
            if (model.Username == null)
            {
                model.Username = "";
            }

            //loopar igenom användarna som hämtades från databasen och kontrollerar så att användarnamnet startar med sökvärdet och att 
            //användarna har valt att vara sökbara, de som matchar värdet läggs till i result listan
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
       
        //skapar vyn för resultatet av sökningen
        public ActionResult SearchUserResult()
        {
            //hämtar ut TempDatan som skickades med från ovanståend ActionResult och skickar med listan till vyn
            var results = TempData["resultData"] as List<SearchModel>;
            return View(results);
        }

        

    }
}