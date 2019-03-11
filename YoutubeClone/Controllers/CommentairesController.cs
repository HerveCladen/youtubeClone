using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;

namespace YoutubeClone.Controllers
{
    public class CommentairesController : Controller
    {
        private YoutubeCloneDb db = new YoutubeCloneDb();

        // GET: Commentaires
        public ActionResult Index()
        {
            var commentaires = db.Commentaires.Include(c => c.Auteur).Include(c => c.Video);
            return View(commentaires.ToList());
        }

        // GET: Commentaires/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commentaire commentaire = db.Commentaires.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return View(commentaire);
        }

        // GET: Commentaires/Create
        public ActionResult Create()
        {
            ViewBag.Chaine_FK = new SelectList(db.Chaines, "ChaineId", "Name");
            ViewBag.Video_FK = new SelectList(db.Videos, "VideoId", "Name");
            return View();
        }

        // POST: Commentaires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Contenu")] Commentaire commentaire, int Utilisateur_FK, int Video_FK)
        {
            if (ModelState.IsValid)
            {
                //try
                {
                    var loggedIn = db.Utilisateurs.FirstOrDefault(p => p.Username == User.Identity.Name);
                    Commentaire c = new Commentaire();
                    c.Contenu = commentaire.Contenu;
                    c.Auteur = db.Utilisateurs.Find(loggedIn.UtilisateurId);
                    c.Utilisateur_FK = loggedIn.UtilisateurId;
                    c.Video = db.Videos.Find(Video_FK);
                    c.Video_FK = Video_FK;
                    db.Commentaires.Add(c);
                    db.Utilisateurs.Find(Utilisateur_FK).Commentaires.Add(c);
                    db.Videos.Find(Video_FK).Commentaires.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Videos", new { id = c.Video_FK });
                } //catch (Exception e) {
                    //return RedirectToAction("Details", "Videos", new { id = Video_FK });
                //}
            }

            ViewBag.Utilisateur_FK = new SelectList(db.Chaines, "ChaineId", "Name", commentaire.Utilisateur_FK);
            ViewBag.Video_FK = new SelectList(db.Videos, "VideoId", "Name", commentaire.Video_FK);
            return View(commentaire);
        }

        // GET: Commentaires/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commentaire commentaire = db.Commentaires.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.Chaine_FK = new SelectList(db.Chaines, "ChaineId", "Name", commentaire.Utilisateur_FK);
            ViewBag.Video_FK = new SelectList(db.Videos, "VideoId", "Name", commentaire.Video_FK);
            return View(commentaire);
        }

        // POST: Commentaires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentaireId,Contenu,DatePublication,Chaine_FK,Video_FK")] Commentaire commentaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentaire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Chaine_FK = new SelectList(db.Chaines, "ChaineId", "Name", commentaire.Utilisateur_FK);
            ViewBag.Video_FK = new SelectList(db.Videos, "VideoId", "Name", commentaire.Video_FK);
            return View(commentaire);
        }

        // GET: Commentaires/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commentaire commentaire = db.Commentaires.Find(id);
            if (commentaire == null)
            {
                return HttpNotFound();
            }
            return View(commentaire);
        }

        // POST: Commentaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Commentaire commentaire = db.Commentaires.Find(id);
            db.Commentaires.Remove(commentaire);
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
