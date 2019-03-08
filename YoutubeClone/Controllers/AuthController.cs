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
            return View(new LoginForm());
        }

        // POST: Login
        [HttpPost, AllowAnonymous]
        public ActionResult Login(LoginForm f, string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                // pour verifier si le username existe dans la bd et pour voir si le pass est valide
                string formPass = Profil.Cryptage(f.HashPassword);
                var user = db.Utilisateurs.FirstOrDefault(u => u.Username == f.UserName && u.HashPassword == formPass);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(f.UserName, f.RemindMe);
                    return this.Redirect(ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Le nom d'utilisateur n'exite pas ou le mot de passe est invalide");
                    return View(f);
                }
            }
            else
            {
                return View(f);
            }
        }

        // GET: SignUp
        [AllowAnonymous]
        public ActionResult SignUp(string ReturnUrl = "") {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new Inscription());
        }

        // POST: SignUp
        [HttpPost, AllowAnonymous]
        public ActionResult SignUp(Inscription i, string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;

            var inscrip = db.Utilisateurs.FirstOrDefault(model => model.Username == i.UserName);
            var inscrip2 = db.Utilisateurs.FirstOrDefault(model => model.Courriel == i.Email);

            if (ModelState.IsValid && inscrip == null && inscrip2 == null)
            {
                db.Utilisateurs.Add(new Utilisateur
                {
                    Username = i.UserName,
                    HashPassword = Profil.Cryptage(i.HashPassword),
                    Courriel = i.Email,
                    IsAdmin = false
                });
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(i.UserName, true);
            }
            else
            {
                // tres laid et mediocre... mais efficace
                if (inscrip != null)
                    ModelState.AddModelError("", "Le nom d'utilisateur est déjà pris.");
                else if (inscrip2 != null)
                    ModelState.AddModelError("", "Le courriel est déjà pris.");

                return View(i);
            }
            return this.Redirect(ReturnUrl);
        }
        [Authorize]
        public ActionResult Logout(string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            FormsAuthentication.SignOut();
            return this.Redirect(ReturnUrl);
        }
    }
}