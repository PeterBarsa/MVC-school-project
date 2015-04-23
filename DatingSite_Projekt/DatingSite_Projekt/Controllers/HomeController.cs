using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DatingSite_Projekt.Models;
using DatingSite_Projekt.Resources;
using Dating_data.Repository;

namespace DatingSite_Projekt.Controllers
{

    public class HomeController : Controller
    {
        // Hämtar vyn för index
        
        public ActionResult Index()
        {
            return View();
        }
        // Hämtar vyn för login
        public ActionResult Login()
        {
            return View();
        }

        // Hämtar en modell från vyn Login
        [HttpPost]
        public ActionResult Login(HomeLoginModel model)
        {
            //kollar så att att allt stämmer med modellen, ex. annotations osv och ifall det inte stämmer skickas man tillbaka till vyn
            //och får felmeddelanden från modellen.
            if (!ModelState.IsValid)
            {
                return View();
            }

            //hämtar informationen från användaren som passar överrens med användarnamnet och lösenordet
            var user = UserRepositories.TestLogIn(model.Username, model.Password);

            //detta sker ifall användaren inte finns/ifall man skrivit något fel då tidigare metod inte hittade informationen och returnerade
            //ett null värde. Metoden under skriver ett felmeddelande som är lokalserat till vyn
            if (user == null)
            {
                ModelState.AddModelError("", login.error);
                return View(model);
            }
            //ifall allt går som det ska skapas en kaka som lagrar användarens id som namn och man returneras till sin profilvy
            //i en annan kontroller
            FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);

            return RedirectToAction("Profile", "Account");
        }

        // Hämtar vyn för Register
        public ActionResult Register()
        {
            return View();
        }

        //använder en model som skickats från vyn register
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
            foreach (var user in comparison)
            {
                if (user.Username.Equals(model.Username))
                {
                    ModelState.AddModelError("", "Username already exists!");
                    return View(model);
                }
            }

            //ifall allt går bra så sparas den nya användaren och skickar personen till login vyn
            UserRepositories.AddNewUser(model.Username, model.Password, model.City, model.Country, model.Email, model.Age, model.Id);
            return RedirectToAction("Login", "Home");
        }

        //ett actionresult för att ändra lokaliseringsvärdet på login sidan, tar emot en sträng language från vyn.
        public ActionResult ChangeCulture(string language)
        {
            //använder strängen för att skapa en httpkaka som variabeln resourceLanguage
            var resourceLanguage = new HttpCookie("lang", language) { HttpOnly = true };
            //lägger till kakan i den nuvarande http sessionen
            Response.AppendCookie(resourceLanguage);
            //returnerar användaren till Loginsidan(var man kommer ifrån) med en ny culture av typen man valt (sv eller en).
            return RedirectToAction("Login", "Home", new { culture = resourceLanguage });

        }
    }
}