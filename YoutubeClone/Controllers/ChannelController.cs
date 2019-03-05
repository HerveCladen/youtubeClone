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
        public ActionResult ChainesUtilisateurs(Utilisateur u)
        {
            var Chaines = db.Chaines;
            return View("~/Views/Channel/ChainesUtilisateurs.cshtml", Chaines.Where(c => c.Utilisateur_FK == u.UtilisateurId));
        }
    }
}