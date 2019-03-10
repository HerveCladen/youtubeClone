using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
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
            // pour eviter le login a un user deja identifier
            // aussi par url
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

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
                var user = db.Utilisateurs.FirstOrDefault(u => u.Username == f.UserName);

                if (user != null && user.HashPassword == formPass)
                {
                    FormsAuthentication.SetAuthCookie(f.UserName, f.RemindMe);
                    return this.Redirect(ReturnUrl);
                }
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "Le nom d'utilisateur n'exite pas");
                    return View(f);
                }
                if (user.HashPassword != formPass)
                    ModelState.AddModelError("HashPassword", "Le mot de passe n'est pas valide");
            }
            return View(f);
        }

        // GET: SignUp
        [AllowAnonymous]
        public ActionResult SignUp(string ReturnUrl = "")
        {
            // pour eviter le SignUp a un user deja identifier
            // aussi via url
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = ReturnUrl;
            return View(new Inscription());
        }

        // POST: SignUp
        [HttpPost, AllowAnonymous]
        public ActionResult SignUp(Inscription i, string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;

            //pour verifier si le username et le email existe deja dans la bd
            var inscripUserName = db.Utilisateurs.FirstOrDefault(model => model.Username == i.UserName);
            var inscripEmail = db.Utilisateurs.FirstOrDefault(model => model.Courriel == i.Email);

            if (ModelState.IsValid && inscripUserName == null && inscripEmail == null)
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
                if (inscripUserName != null)
                    ModelState.AddModelError("UserName", "Le nom d'utilisateur est déjà pris.");
                else if (inscripEmail != null)
                    ModelState.AddModelError("Email", "Le courriel est déjà pris.");

                return View(i);
            }
            return this.Redirect(ReturnUrl);
        }
        // GET: Utilisateurs/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            // ma methode == plus efficace
            var u = db.Utilisateurs.FirstOrDefault(user => user.Username == User.Identity.Name);
            var p = new Profil() { UserName = u.Username, Email = u.Courriel };

            ViewBag.userId = u.UtilisateurId;

            return this.View(p);
        }

        // POST: Utilisateurs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Profil profil, string OldPassConf)
        {
            ViewBag.error = "";

            var user = db.Utilisateurs.FirstOrDefault(model => model.Username == User.Identity.Name);

            if (ModelState.IsValid)
            {
                if (Profil.Cryptage(OldPassConf) == user.HashPassword && Profil.Cryptage(OldPassConf) != Profil.Cryptage(profil.HashPassword))
                {
                    if (user.Username != profil.UserName)
                        user.Username = profil.UserName;

                    if (user.HashPassword != Profil.Cryptage(profil.HashPassword))
                        user.HashPassword = Profil.Cryptage(profil.HashPassword);

                    if (user.Courriel != profil.Email)
                        user.Courriel = profil.Email;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("InfoUser", "Auth", null);
                }
                else if (user.HashPassword == Profil.Cryptage(profil.HashPassword))
                {
                    ModelState.AddModelError("HashPassword", "Veuillez choisir un mot de passe different du precedent");
                }
                else if (Profil.Cryptage(OldPassConf) != user.HashPassword)
                {
                    ModelState.AddModelError("OldPassConf", "Veuillez entrez le bon mot de passe");
                }
            }
            return this.View(profil);
        }
        [Authorize]
        public ActionResult Logout(string ReturnUrl = "")
        {
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        // GET: InfoUser
        [Authorize]
        public ActionResult InfoUser()
        {
            var user = db.Utilisateurs.FirstOrDefault(model => model.Username == User.Identity.Name);
            Profil info = new Profil { UserName = user.Username, Email = user.Courriel };

            return View(info);
        }
    }
}