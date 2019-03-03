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
    public class RechercheController : Controller {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        // GET: Recherche
        
        public ActionResult Index(bool isFromCat = false, string categorie = "All",string search="",string isChaine="off", string isVideo = "off", string uploadFrame="All",string sortBy="Popular", int page=1) {
           /*
            * page est automatique
            * video/chaine peuvent etre false ou true
            *categorie doit etre la partie de l'enum
            *sort by defaut cest popular sinon upload 
            *upload date prend all/day/week/month/year et s'automatise
            * test le match dans le title/desc/tags
            */
            if (search != "" || isFromCat) {
                var Videos = db.Videos;
                var Chaines = db.Chaines;
                IQueryable<Video> queriedVideos;
                IQueryable<Chaine> queriedChaines;

                queriedVideos = Videos.Where(V=>V.Name.Contains(search) || V.Tags_Video.Contains(search) ||V.Description.Contains(search));
                queriedChaines = Chaines.Where(C=>C.Name.Contains(search) || C.Tags_Chaine.Contains(search) || C.Description.Contains(search));

                if (categorie != "All") {
                    queriedVideos = queriedVideos.Where(V => V.Categorie_Video.ToString() == categorie);
                    queriedChaines = queriedChaines.Where(C=>C.Categorie_Chaine.ToString() == categorie);
                }

                if (sortBy == "Popular") {
                    queriedVideos = queriedVideos.OrderByDescending(C => C.Views);
                    queriedChaines = queriedChaines.OrderByDescending(C => C.Videos.Sum(t => t.Views));
                } else if (sortBy == "UploadDate") {
                    queriedVideos = queriedVideos.OrderByDescending(C => C.DatePublished);
                    queriedChaines = queriedChaines.OrderByDescending(C => C.Videos.Max(t => t.DatePublished));
                } else {
                    //sort by popularity by default
                    queriedVideos = queriedVideos.OrderByDescending(C => C.Views);
                    queriedChaines = queriedChaines.OrderByDescending(C => C.Videos.Sum(t => t.Views));
                }


                if (uploadFrame != "All") {
                    DateTime CompareDate = DateTime.Today;
                    if (uploadFrame == "Day") {
                        CompareDate.AddDays(-1);
                    } else if (uploadFrame == "Week") {
                        CompareDate.AddDays(-7);
                    } else if (uploadFrame == "Month") {
                        CompareDate.AddDays(-30);
                    } else if (uploadFrame == "Year") {
                        CompareDate.AddDays(-365);
                    }
                    queriedVideos = queriedVideos.Where(C => C.DatePublished > CompareDate);
                    queriedChaines = queriedChaines.Where(C => C.Videos.Max(t => t.DatePublished) > CompareDate);
                }


                ViewBag.Videos = queriedVideos;
                ViewBag.Chaines = queriedChaines;
                if (queriedVideos.Count() != 0)
                    ViewBag.StatusVideo = true;
                else
                    ViewBag.StatusVideo = false;
                if (queriedChaines.Count() != 0)
                    ViewBag.StatusChaine = true;
                else
                    ViewBag.StatusChaine = false;
                ViewBag.Recherche = search+"  of type " +categorie+"  within "+ uploadFrame+"  sorted by "+sortBy;
                if (!(isVideo.ToUpper()=="ON")) {
                    ViewBag.Videos = new List<Video>();
                    ViewBag.StatusVideo = false;
                }
                if (!(isChaine.ToUpper()== "ON")) {
                    ViewBag.Chaines = new List<Chaine>();
                    ViewBag.StatusChaine = false;
                }
                return View("~/Views/Recherche/Recherche.cshtml");
            } else {
                ViewBag.StatusVideo = false;
                ViewBag.StatusChaine = false;
                ViewBag.Videos = new List<Video>();
                ViewBag.Chaines = new List<Chaine>();
                return View("~/Views/Recherche/Recherche.cshtml");
            }
        }
        
    }
}