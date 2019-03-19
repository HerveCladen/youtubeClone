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
using YoutubeClone.Models.View_Models;
using System.IO;

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
                if (user.Historique.Where(c => c.VideoId == video.VideoId).Count() == 0)
                    user.Historique.Add(video);
            }
            if (User.Identity.IsAuthenticated && User.Identity.Name == utilisateurs.Find(chaines.Find(videos.Find(id).Chaine_FK).Utilisateur_FK).Username)
            /*if (User.Identity.IsAuthenticated && User.Identity.Name == utilisateurs.Where(c => c.UtilisateurId == chaines.Where(C => C.ChaineId == utilisateurs.Where(D => D.Username == User.Identity.Name).FirstOrDefault().UtilisateurId).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username)*/ {
                ViewBag.EditOK = true;
            } else {
                ViewBag.EditOK = false;
            }
            db.SaveChanges();
            ViewBag.User = db.Utilisateurs.FirstOrDefault(user => user.Username == User.Identity.Name);
            return View("~/Views/Videos/VideoViewer.cshtml", video);
        }

        // GET: Videos1/Create
        [Authorize]
        public ActionResult Create(int id) {
            return View(new VideoCreate {Chaine_FK=id});
        }

        // POST: Videos1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Description,Categorie_Video,Chaine_FK,Tags_Video")] VideoCreate videoCreate, HttpPostedFileBase videoUP, HttpPostedFileBase posterUP ) {
            if (ModelState.IsValid) {
                try {
                    Video video = new Video();
                    video.Name = videoCreate.Name;
                    video.Description = videoCreate.Description;
                    video.Categorie_Video = videoCreate.Categorie_Video;
                    video.Tags_Video = videoCreate.Tags_Video;
                    video.Channel = db.Chaines.Find(videoCreate.Chaine_FK);
                    video.Chaine_FK = videoCreate.Chaine_FK;
                    db.Videos.Add(video);
                    db.SaveChanges();
                    video.ThumbnailPath = "/Content/Thumbnails/" + video.VideoId + Path.GetExtension(posterUP.FileName);
                    video.VideoPath =  "/Content/Videos/" + video.VideoId + Path.GetExtension(videoUP.FileName);
                    posterUP.SaveAs(Server.MapPath("~") + "Content\\Thumbnails\\" + video.VideoId + Path.GetExtension(posterUP.FileName));
                    videoUP.SaveAs(Server.MapPath("~") + "Content\\Videos\\" + video.VideoId + Path.GetExtension(videoUP.FileName));
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = video.VideoId });
                } catch (Exception e) {
                    return View(videoCreate);
                }
            }
            
            return View(videoCreate);
        }

        // GET: Videos1/Edit/5
        [Authorize]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            VideoEdit videoEdit = new VideoEdit {VideoId= video.VideoId,Description = video.Description,Tags_Video=video.Tags_Video,Name=video.Name,Categorie_Video=video.Categorie_Video };
            if (video == null) {
                return HttpNotFound();
            }
            return View(videoEdit);
        }

        // POST: Videos1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,Name,Description,Categorie_Video,Tags_Video")] VideoEdit videoEdit) {
            var utilisateurs = db.Utilisateurs;
            var chaines = db.Chaines;
            if (ModelState.IsValid) {
                if (db.Utilisateurs.Find(db.Chaines.Find(db.Videos.Find(videoEdit.VideoId).Chaine_FK).Utilisateur_FK).Username == User.Identity.Name) {
                    Video video = db.Videos.Find(videoEdit.VideoId);
                    video.Name = videoEdit.Name;
                    video.Description = videoEdit.Description;
                    video.Categorie_Video = videoEdit.Categorie_Video;
                    video.Tags_Video = videoEdit.Tags_Video;
                    db.Entry(video).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = video.VideoId });
                } else {
                    ModelState.AddModelError("Name", "You may not edit a video owned by someone else");
                    return View(videoEdit);
                }
            }
            return View(videoEdit);
        }

        // GET: Videos1/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, string error ="") {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null) {
                return HttpNotFound();
            }
            ViewBag.Error = error;
            return View(video);
        }

        // POST: Videos1/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            var utilisateurs = db.Utilisateurs;
            var chaines = db.Chaines;
            if (db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin
                ||
                db.Utilisateurs.Find(db.Chaines.Find(db.Videos.Find(id).Chaine_FK).Utilisateur_FK).Username == User.Identity.Name
                ) {
                Video video = db.Videos.Find(id);
                foreach (Commentaire c in video.Commentaires.ToList())
                {
                    db.Commentaires.Remove(c);
                }
                try {
                    System.IO.File.Delete(Server.MapPath("~") + video.ThumbnailPath);
                    System.IO.File.Delete(Server.MapPath("~") + video.VideoPath);
                } catch (Exception e) {
                }
                db.Videos.Remove(video);
                db.SaveChanges();
            } else {
                return RedirectToAction("Delete", "Videos",new {error="You may not delete a video owned by someone else"});
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

        public static string GetTimeDifference(DateTime d)
        {
            int diff = Convert.ToInt32(Math.Floor(DateTime.Now.Subtract(d).TotalDays));
            if (diff == 0) return "Aujourd'hui";
            else if (diff == 1) return "Hier";
            else if (diff < 7) return "Il y a " + diff + " jours";
            else if (diff < 30) return "Il y a " + (diff / 7) + " semaines";
            else if (diff < 365) return "Il y a " + (((DateTime.Now.Year - d.Year) * 12) + DateTime.Now.Month - d.Month) + " mois";
            else return "Il y a " + (DateTime.Now.Year - d.Year) + " ans";
        }

        [Authorize]
        public ActionResult Like(int id)
        {
            var user = db.Utilisateurs.FirstOrDefault(p => p.Username == User.Identity.Name);
            var video = db.Videos.Find(id);
            if (user.DislikedVideos.Contains(video)) {
                video.Dislikes--; user.DislikedVideos.Remove(video);
            }
            if (!user.LikedVideos.Contains(video)) {
                video.Likes++;
                user.LikedVideos.Add(video);
            } else {
                video.Likes--;
                user.LikedVideos.Remove(video);
            }
            db.SaveChanges();
            
            return RedirectToAction("Details", "Videos", new { id });
        }

        [Authorize]
        public ActionResult Dislike(int id)
        {
            var user = db.Utilisateurs.FirstOrDefault(p => p.Username == User.Identity.Name);
            var video = db.Videos.Find(id);
            if (user.LikedVideos.Contains(video)) {
                video.Likes--; user.LikedVideos.Remove(video);
            }
            if (!user.DislikedVideos.Contains(video)) {
                video.Dislikes++;
                user.DislikedVideos.Add(video);
            }
            else
            {
                video.Dislikes--;
                user.DislikedVideos.Remove(video);
            }
            db.SaveChanges();
            return RedirectToAction("Details", "Videos", new { id });
        }

        public static int RatioLD(int likes, int dislikes)
        {
            double l = likes;
            double d = dislikes;
            if (dislikes != 0) {
                return Convert.ToInt32(l / (l + d) * 100);
            } else {
                return 100;
            }
        }
    }
}
