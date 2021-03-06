﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatingSite_Projekt.Helpers;
using DatingSite_Projekt.Models;
using DatingSite_Projekt.Resources;
using Dating_data.Repository;

namespace DatingSite_Projekt.Controllers
{

    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {

            var userList = UserRepositories.GetUsers();
            var frontList = RandomFourMembers(userList);

           
            return View(frontList);
        }

        private static List<AccountUserModel> RandomFourMembers(List<User> userList)
        {
            var added = new List<int>();
            var resultList = new List<AccountUserModel>();

            //loopar igenom tills 4 olika användare har hämtats som skickas till vyn
            do
            {
                
                var r = new Random();
                var user = userList[r.Next(userList.Count)];
                if (added.Contains(user.Id))
                {
                    //gör inget ifall det finns så loopen fortsätter
                }
                else
                {

                    var descriptionForUser = DescriptionRepository.GetDescription(user.Id);
                    var userModel = new AccountUserModel
                    {
                        Username = user.Username,
                        AboutMe = descriptionForUser.AboutMe,
                        Age = descriptionForUser.Age ?? default(int),
                        UserId = user.Id
                    };
                    resultList.Add(userModel);
                    added.Add(user.Id);
                }
                
            }
            while (resultList.Count < 4);

            return resultList;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(HomeLoginModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = UserRepositories.TestLogIn(model.Username, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", login.error);
                return View(model);
            }
  
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

            return RedirectToAction("Profile", "Account");
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            //hämtar en lista av alla användare
            var comparison = UserRepositories.GetUsers();

            //kollar så att att allt stämmer med modellen, ex. annotations osv och ifall det inte stämmer skickas man tillbaka till vyn
            //och får felmeddelanden från modellen.
            if (!ModelState.IsValid)
            {
                return View();
            }

            //loopar igenom comparisonlistan och jämför alla användarnamn och kollar så att användarnamnen är unika
            //ifall användarnamnet redan finns blir man skickad tillbaka till vyn med ett felmeddelande.
            if (comparison.Any(user => user.Username.Equals(model.Username)))
            {
                ModelState.AddModelError("", English.User_Exists);
                return View(model);
            }

            UserRepositories.AddNewUser(model.Username, model.Password, model.City, model.Country, model.Email, model.Age, model.Id);
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ChangeCulture(string language)
        {
            //använder strängen för att skapa en httpkaka som variabeln resourceLanguage
            var resourceLanguage = new HttpCookie("lang", language) { HttpOnly = true };
            //lägger till kakan i den nuvarande http sessionen
            Response.AppendCookie(resourceLanguage);
            return RedirectToAction("Login", "Home", new { culture = resourceLanguage });

        }
    }
}