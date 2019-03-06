using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using System.Net;

namespace YoutubeClone.Controllers
{
    public class VideosController : Controller {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        public ActionResult RecentlyUploaded() {
            var Videos= db.Videos;
            //retourne les 5 plus recent
            return PartialView("~/Views/Shared/_RecentlyUploaded.cshtml", Videos.OrderByDescending(C => C.DatePublished).Take(5));
        }

        public ActionResult MostPopularVideos() {
            var Videos = db.Videos;
            return PartialView("~/Views/Shared/_MostPopularVideos.cshtml", Videos.OrderByDescending(C => C.Views).Take(25));
        }

        public ActionResult ShowVideo(Video  v) {
            return PartialView("~/Views/Shared/_VideoThumbnail.cshtml", v);
        }

        // GET: Default/Details/5
        public ActionResult Details(int id) {
            var videos = db.Videos;
            var utilisateurs = db.Utilisateurs;
            var chaines = db.Chaines;
            var video = videos.Where(V => V.VideoId == id).First();
            video.Views++;
            if (User.Identity.IsAuthenticated) {
                Utilisateur user = db.Utilisateurs.First(c => c.Username == User.Identity.Name);
                //Faire un model builder
                if (user.Historique.Where(c => c.VideoId == video.VideoId).Count() == 0)
                    user.Historique.Add(video);
                    //user.Historique.Add(new VideoUtilisateur { VideoID = video.VideoId, Video = video });
                /*if (video.Viewers.Where(c => c.UtilisateurID == user.UtilisateurId).Count() == 0)
                    video.Viewers.Add(new VideoUtilisateur { UtilisateurID = user.UtilisateurId, Utilisateur = user });
            */}
            
            if (User.Identity.IsAuthenticated && User.Identity.Name == utilisateurs.Where(c => c.UtilisateurId == chaines.Where(C => C.ChaineId == utilisateurs.Where(D => D.Username == User.Identity.Name).FirstOrDefault().UtilisateurId).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username) {
                ViewBag.EditOK = true;
            } else {
                ViewBag.EditOK = false;
            }
            db.SaveChanges();
            return View("~/Views/Videos/VideoViewer.cshtml", video);
        }

        // GET: Videos1/Create
        [Authorize]
        public ActionResult Create() {
            ViewBag.Chaine_FK = new SelectList(db.Chaines, "ChaineId", "Name");
            return View();
        }

        // POST: Videos1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "VideoId,Views,Name,Description,Categorie_Video,DatePublished,Chaine_FK,Tags_Video,VideoPath,ThumbnailPath")] Video video) {
            if (ModelState.IsValid) {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Chaine_FK = new SelectList(db.Chaines, "ChaineId", "Name", video.Chaine_FK);
            return View(video);
        }

        // GET: Videos1/Edit/5
        [Authorize]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null) {
                return HttpNotFound();
            }
            ViewBag.Chaine_FK = db.Videos.Find(id).Chaine_FK;
            return View(video);
        }

        // POST: Videos1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,Views,Name,Description,Categorie_Video,DatePublished,Chaine_FK,Tags_Video,VideoPath,ThumbnailPath")] Video video) {
            if (ModelState.IsValid) {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
                return View("~/Views/Channel/ChainesUtilisateurs.cshtml", db.Chaines.Where(c => c.Utilisateur_FK == user));
            }
            ViewBag.Chaine_FK = db.Videos.Find(video).Chaine_FK;
            return View(video);
        }

        // GET: Videos1/Delete/5
        [Authorize]
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null) {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos1/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            var utilisateurs = db.Utilisateurs;
            var chaines = db.Chaines;
            if ( User.Identity.Name == utilisateurs.Where(c => c.UtilisateurId == chaines.Where(C => C.ChaineId == utilisateurs.Where(D => D.Username == User.Identity.Name).FirstOrDefault().UtilisateurId).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username) {
                Video video = db.Videos.Find(id);
                db.Videos.Remove(video);
                db.SaveChanges();
            }
            var user = utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("~/Views/Channel/ChainesUtilisateurs.cshtml", db.Chaines.Where(c => c.Utilisateur_FK == user));
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
