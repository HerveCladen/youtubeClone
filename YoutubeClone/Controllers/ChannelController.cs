using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;

namespace YoutubeClone.Controllers
{
    public class ChannelController : Controller {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        public ActionResult MostPopularChannels() {
            var Chaines = db.Chaines;
            //retourne les 5 plus populaires 
            return PartialView("~/Views/Shared/_MostPopularChannels.cshtml", Chaines.OrderByDescending(C => C.Videos.Sum(t => t.Views)).Take(5));
        }
        public ActionResult MostPopularVideosOfChannels(Chaine c) {
            //retourne les 5 plus populaires 
            ViewBag.Chaine = c.Name;
            ViewBag.ChaineId = c.ChaineId;
            var videosChaine = db.Videos.Where(b => b.Chaine_FK == c.ChaineId);
            return PartialView("~/Views/Shared/_Channel.cshtml", videosChaine.OrderByDescending(C => C.Views).Take(5));
        }
        public ActionResult VideosOfChannelsByUpload(Chaine c) {
            //retourne les 5 plus populaires 
            ViewBag.Chaine = c.Name;
            var videosChaine = db.Videos.Where(b => b.Chaine_FK == c.ChaineId);
            return PartialView("~/Views/Shared/_RecentlyUploaded.cshtml", videosChaine.OrderByDescending(C => C.DatePublished));
        }

        // GET: Default/Details/5
        public ActionResult Details(int id) {
            var Chaine = db.Chaines.Where(C => C.ChaineId == id).First();
            return View("~/Views/Channel/ChannelViewer.cshtml", Chaine);
        }

        //Ne retourne aucune chaîne
        [Authorize]
        public ActionResult ChainesUtilisateurs()
        {
            var Chaines = db.Chaines;
            var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("~/Views/Channel/ChainesUtilisateurs.cshtml", Chaines.Where(c => c.Utilisateur_FK == user));
        }

        // GET: Chaines/Create
        public ActionResult Create() {
            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View();
        }

        // POST: Chaines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Description,Utilisateur_FK,Categorie_Chaine,Tags_Chaine")] Chaine chaine) {
            if (ModelState.IsValid) {
                db.Chaines.Add(chaine);
                db.SaveChanges();
                return RedirectToAction("ChainesUtilisateurs");
            }

            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("Create.cshtml",chaine);
        }

        // GET: Chaines/Edit/5
        [Authorize]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chaine chaine = db.Chaines.Find(id);
            if (chaine == null) {
                return HttpNotFound();
            }
            ViewBag.Utilisateur_FK = new SelectList(db.Utilisateurs, "UtilisateurId", "Username", chaine.Utilisateur_FK);
            return View("~/Views/Channel/Edit.cshtml",chaine);
        }

        // POST: Chaines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ChaineId,Name,Description,Utilisateur_FK,Categorie_Chaine,Tags_Chaine")] Chaine chaine) {
            if (ModelState.IsValid) {
                db.Entry(chaine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Utilisateur_FK = new SelectList(db.Utilisateurs, "UtilisateurId", "Username", chaine.Utilisateur_FK);
            return View("~/Views/Channel/Edit.cshtml",chaine);
        }

        // GET: Chaines/Delete/5
        [Authorize]
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chaine chaine = db.Chaines.Find(id);
            if (chaine == null) {
                return HttpNotFound();
            }
            return View("~/Views/Channel/Delete.cshtml",chaine);
        }

        // POST: Chaines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id) {
            Chaine chaine = db.Chaines.Find(id);
            db.Chaines.Remove(chaine);
            db.SaveChanges();
            return Redirect("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}