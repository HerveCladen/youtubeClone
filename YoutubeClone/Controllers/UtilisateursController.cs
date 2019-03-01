using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;

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

        //// GET: Utilisateurs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////POST: Utilisateurs/Create
        ////To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //  //more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "UtilisateurId,Username,Courriel,Password")] Utilisateur utilisateur)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Utilisateurs.Add(utilisateur);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(utilisateur);
        //}

        // GET: Utilisateurs/Edit/5
        [Authorize]
        public ActionResult Edit()
        {
            int UserID = Convert.ToInt32(User.Identity.Name);
            Utilisateur u = db.Utilisateurs.Find(UserID);
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
            ViewBag.error = "";
            ViewBag.ReturnUrl = ReturnUrl;
            u.UtilisateurId = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return this.RedirectToAction(ReturnUrl);
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
