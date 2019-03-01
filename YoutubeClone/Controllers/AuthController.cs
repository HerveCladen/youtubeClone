using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YoutubeClone.Models;
using YoutubeClone.Models.Data_Models;
using YoutubeClone.Models.View_Models;

namespace YoutubeClone.Controllers
{
    public class AuthController : Controller
    {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            return PartialView(new LoginForm());
        }

        // POST: Login
        [HttpPost, AllowAnonymous]
        public ActionResult Login(LoginForm f, string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {

                FormsAuthentication.SetAuthCookie(f.UserName, f.RemindMe);
                return this.RedirectToAction(ReturnUrl);
            }
            else
            {
                return View(f);
            }
        }

        // GET: SignUp
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View(new Inscription());
        }

        // POST: SignUp
        [HttpPost, AllowAnonymous]
        public ActionResult SignUp(Inscription i, string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                db.Utilisateurs.Add(new Utilisateur
                {
                    Username = i.UserName,
                    HashPassword = i.Encodage(i.HashPassword),
                    Courriel = i.Email,
                    isAdmin = false
                });
                FormsAuthentication.SetAuthCookie(i.UserName, true);
            }
            else
            {
                return View(i);
            }
            return this.RedirectToAction(ReturnUrl);
        }
        [Authorize]
        public ActionResult Logout(string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            FormsAuthentication.SignOut();
            return this.RedirectToAction(ReturnUrl);
        }
    }
}