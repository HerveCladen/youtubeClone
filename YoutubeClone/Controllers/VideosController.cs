using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using PagedList;
using PagedList.Mvc;

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
            var video = db.Videos.Where(V => V.VideoId == id).First();
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
            db.SaveChanges();
            return View("~/Views/Videos/VideoViewer.cshtml", video);
        }
    }
}
