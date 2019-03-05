using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using YoutubeClone.Models.View_Models;

namespace YoutubeClone_Bruce.Controllers
{
    public class UtilisateursController : Controller
    {
        private YoutubeCloneDb db = new YoutubeCloneDb();

        // GET: Utilisateurs
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Utilisateurs.ToList());
        }

        // GET: Utilisateurs/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            // ma methode == plus efficace
            var u = db.Utilisateurs.FirstOrDefault(user => user.Username == User.Identity.Name);

            // methode du prof...
            //int UserID = Convert.ToInt32(User.Identity);
            //Utilisateur u = db.Utilisateurs.Find(UserID);
            return this.View(u);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Utilisateur u, string ReturnUrl = "")
        {
            //   ne fonctionne pas comme prevue...
            //ViewBag.error = "";
            //ViewBag.ReturnUrl = ReturnUrl;
            //u.UtilisateurId = Convert.ToInt32(User.Identity);
            var user = db.Utilisateurs.FirstOrDefault(model => model.Username == User.Identity.Name);

            if (ModelState.IsValid)
            {
                if (user.HashPassword != Profil.Cryptage(u.HashPassword))
                    user.HashPassword = Profil.Cryptage(u.HashPassword);
                //**********************************************************************************************************************//
                // la ligne 80 renvoi une exception des qu'il y a une modification.. a voir comment fix
                // System.Data.Entity.Infrastructure.DbUpdateConcurrencyException: 
                //'Store update, insert, or delete statement affected an unexpected number of rows (0).
                //Entities may have been modified or deleted since entities were loaded. See http://go.microsoft.com/fwlink/?LinkId=472540 
                //for information on understanding and handling optimistic concurrency exceptions.'

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return this.RedirectToAction("Index", "Home");
            }
            return this.View(u);
        }

        // GET: Utilisateurs/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(utilisateur);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Historique()
        {
            Utilisateur u = (Utilisateur)User.Identity;
            var user = db.Utilisateurs.Where(c => c.UtilisateurId == u.UtilisateurId);
            return View("~/Views/Videos/Historique.cshtml", user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}