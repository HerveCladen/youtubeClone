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
                Utilisateur temp = (Utilisateur)User.Identity;
                Utilisateur user = db.Utilisateurs.Find(temp.UtilisateurId);
                user.Historique.Add(video);
            }
            db.SaveChanges();
            return View("~/Views/Videos/VideoViewer.cshtml", video);
        }
    }
}
