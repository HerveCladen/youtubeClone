using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using PagedList;
using PagedList.Mvc;
using YoutubeClone.Models.View_Models;

namespace YoutubeClone.Controllers
{
    public class RechercheController : Controller
    {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        // GET: Recherche

        public ActionResult Index(bool isFromCat = false, string categorie = "All", string search = "", string isChaine = "off", string isVideo = "off", string uploadFrame = "All", string sortBy = "Popular",int pageNumber=1)
        {
            /*
             * page est automatique
             * video/chaine peuvent etre false ou true
             *categorie doit etre la partie de l'enum
             *sort by defaut cest popular sinon upload 
             *upload date prend all/day/week/month/year et s'automatise
             * test le match dans le title/desc/tags
             */
            if (search != "" || isFromCat)
            {
                var Videos = db.Videos;
                var Chaines = db.Chaines;
                IQueryable<Video> queriedVideos;
                IQueryable<Chaine> queriedChaines;

                queriedVideos = Videos.Where(V => V.Name.Contains(search) || V.Tags_Video.Contains(search) || V.Description.Contains(search));
                queriedChaines = Chaines.Where(C => C.Name.Contains(search) || C.Tags_Chaine.Contains(search) || C.Description.Contains(search));

                if (categorie != "All")
                {
                    queriedVideos = queriedVideos.Where(V => V.Categorie_Video.ToString() == categorie);
                    queriedChaines = queriedChaines.Where(C => C.Categorie_Chaine.ToString() == categorie);
                }

                if (sortBy == "UploadDate")
                {
                    queriedVideos = queriedVideos.OrderByDescending(C => C.DatePublished);
                    queriedChaines = queriedChaines.OrderByDescending(C => C.Videos.Max(t => t.DatePublished));
                }
                else if (sortBy == "Relevance" && !isFromCat)
                {
                    //sort by relevance but only if it does not come from category as it would mean the search parameter is empty
                    List<RechercheCoefficient> videoCoefficients = new List<RechercheCoefficient>();
                    foreach (Video v in queriedVideos)
                    {
                        videoCoefficients.Add(new RechercheCoefficient(search, v.Name, v.Description, v.Tags_Video, v.VideoId));
                    }
                    List<RechercheCoefficient> chaineCoefficients = new List<RechercheCoefficient>();
                    foreach (Chaine v in queriedChaines)
                    {
                        chaineCoefficients.Add(new RechercheCoefficient(search, v.Name, v.Description, v.Tags_Chaine, v.ChaineId));
                    }
                    videoCoefficients.OrderByDescending(v => v.coefficient);
                    queriedVideos = queriedVideos.ToList().OrderBy(C => videoCoefficients.IndexOf(videoCoefficients.Find(A => A.id == C.VideoId))).ThenBy(C => C.Views).ToList().AsQueryable();
                    queriedChaines = queriedChaines.ToList().OrderBy(C => chaineCoefficients.IndexOf(chaineCoefficients.Find(A => A.id == C.ChaineId))).ThenBy(C => C.Videos.Sum(t => t.Views)).ToList().AsQueryable();
                }
                else
                {
                    //sortby popular by default or if comes from category while relevance is manually put in
                    queriedVideos = queriedVideos.OrderByDescending(C => C.Views);
                    queriedChaines = queriedChaines.OrderByDescending(C => C.Videos.Sum(t => t.Views));
                }


                if (uploadFrame != "All")
                {
                    DateTime CompareDate = DateTime.Today;
                    if (uploadFrame == "Day")
                    {
                        CompareDate.AddDays(-1);
                    }
                    else if (uploadFrame == "Week")
                    {
                        CompareDate.AddDays(-7);
                    }
                    else if (uploadFrame == "Month")
                    {
                        CompareDate.AddDays(-30);
                    }
                    else if (uploadFrame == "Year")
                    {
                        CompareDate.AddDays(-365);
                    }
                    queriedVideos = queriedVideos.Where(C => C.DatePublished > CompareDate);
                    queriedChaines = queriedChaines.Where(C => C.Videos.Max(t => t.DatePublished) > CompareDate);
                }

                //HANDLE PAGES
                int nbParPageVideo = 9;
                int nbParPageChaine = 3;

                if (queriedVideos.Count() / nbParPageVideo >= queriedChaines.Count() / nbParPageChaine) {
                    ViewBag.EndPage = ViewBag.LastPage = "/Recherche/Index?search=" + search + "&isChaine=" + isChaine + "&isVideo=" + isVideo + "&uploadFrame=" + uploadFrame + "&sortBy=" + sortBy + "&categorie=" + categorie + "&isFromCat=" + isFromCat + "&pageNumber=" + Math.Ceiling((double)queriedVideos.Count() / nbParPageVideo);
                } else {
                    ViewBag.EndPage = ViewBag.LastPage = "/Recherche/Index?search=" + search + "&isChaine=" + isChaine + "&isVideo=" + isVideo + "&uploadFrame=" + uploadFrame + "&sortBy=" + sortBy + "&categorie=" + categorie + "&isFromCat=" + isFromCat + "&pageNumber=" + Math.Ceiling((double)queriedChaines.Count() / nbParPageChaine);
                }
                ViewBag.FirstPage = ViewBag.LastPage = "/Recherche/Index?search=" + search + "&isChaine=" + isChaine + "&isVideo=" + isVideo + "&uploadFrame=" + uploadFrame + "&sortBy=" + sortBy + "&categorie=" + categorie + "&isFromCat=" + isFromCat + "&pageNumber=1" ;

                if (queriedVideos.Count() > nbParPageVideo * pageNumber || queriedChaines.Count() > nbParPageChaine * pageNumber) {
                    ViewBag.NextPage = "/Recherche/Index?search="+search+ "&isChaine=" + isChaine + "&isVideo=" + isVideo + "&uploadFrame=" + uploadFrame + "&sortBy=" + sortBy + "&categorie=" + categorie + "&isFromCat=" + isFromCat + "&pageNumber="+(pageNumber+1);
                } else {
                    ViewBag.NextPage = "#";
                }
                if (queriedVideos.Count() <= nbParPageVideo * pageNumber || queriedChaines.Count() <= nbParPageChaine * pageNumber) {
                    ViewBag.LastPage = "/Recherche/Index?search=" + search + "&isChaine=" + isChaine + "&isVideo=" + isVideo + "&uploadFrame=" + uploadFrame + "&sortBy=" + sortBy + "&categorie=" + categorie + "&isFromCat=" + isFromCat + "&pageNumber=" + (pageNumber-1);
                } else {
                    ViewBag.LastPage = "#";
                }
                pageNumber-=1;

                queriedVideos = queriedVideos.Skip(nbParPageVideo * pageNumber).Take(nbParPageVideo);
                queriedChaines = queriedChaines.Skip(nbParPageChaine * pageNumber).Take(nbParPageChaine);
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
                ViewBag.Recherche = search + "  of type " + categorie + "  within " + uploadFrame + "  sorted by " + sortBy;
                if (!(isVideo.ToUpper() == "ON"))
                {
                    ViewBag.Videos = new List<Video>();
                    ViewBag.StatusVideo = false;
                }
                if (!(isChaine.ToUpper() == "ON"))
                {
                    ViewBag.Chaines = new List<Chaine>();
                    ViewBag.StatusChaine = false;
                }
                return View("~/Views/Recherche/Recherche.cshtml");
            }
            else
            {
                ViewBag.StatusVideo = false;
                ViewBag.StatusChaine = false;
                ViewBag.Videos = new List<Video>();
                ViewBag.Chaines = new List<Chaine>();
                return View("~/Views/Recherche/Recherche.cshtml");
            }
        }




        public ActionResult Recommended() {
            var videos = db.Videos;
            var users = db.Utilisateurs;
            var historique = users.Find(users.Where(c=>c.Username == User.Identity.Name).First().UtilisateurId).Historique.AsEnumerable();
            List<RechercheCoefficient> videoCoefficients = new List<RechercheCoefficient>();
            IEnumerable<string> searchWords = new List<string>();
            foreach (Video v in historique) {
                //only factor in the name of the videos you watched
                searchWords = searchWords.Concat((v.Name /*+ v.Tags_Video + v.Description*/).Split(' ').ToList());
            }

            foreach (Video v in videos) {
                var tempcoefficient = new RechercheCoefficient(v.VideoId);
                foreach (string s in searchWords) { 
                    var temp = new RechercheCoefficient(s, v.Name, v.Description, v.Tags_Video, v.VideoId);
                    tempcoefficient.coefficient += temp.coefficient;
                }
                videoCoefficients.Add(tempcoefficient);
            }

            if (historique.Count() == 0) {
                ViewBag.HasHistorique = false;
            } else {
                ViewBag.HasHistorique = true;
            }

            videoCoefficients = videoCoefficients.OrderByDescending(v => v.coefficient).ToList();
            List<int> RecommendedVideos = new List<int>();
            for (int v = 0; v < videoCoefficients.Count(); v++) {
                bool isOk = true;
                foreach (Video vid in historique) {
                    if (vid.VideoId == videoCoefficients[v].id)
                        isOk = false;
                }
                if(isOk)
                    RecommendedVideos.Add(videoCoefficients[v].id);
            }
            var query = videos.Where(c => RecommendedVideos.Contains(c.VideoId)).Take(5).AsEnumerable();

            if (query.Count() == 0) {
                ViewBag.MoreToSee = false;
            } else {
                ViewBag.MoreToSee = true;
            }

            return PartialView("~/Views/Shared/_Recommended.cshtml", query);
        }

        public ActionResult ChannelRecommended(int id) {
            var videos = db.Videos.Where(c=>c.Chaine_FK == id);
            var users = db.Utilisateurs;
            var historique = users.Find(users.Where(c => c.Username == User.Identity.Name).First().UtilisateurId).Historique.AsEnumerable();
            List<RechercheCoefficient> videoCoefficients = new List<RechercheCoefficient>();
            IEnumerable<string> searchWords = new List<string>();
            foreach (Video v in historique) {
                searchWords = searchWords.Concat((v.Name + v.Tags_Video + v.Description).Split(' ').ToList());
            }

            foreach (Video v in videos) {
                var tempcoefficient = new RechercheCoefficient(v.VideoId);
                foreach (string s in searchWords) {
                    var temp = new RechercheCoefficient(s, v.Name, v.Description, v.Tags_Video, v.VideoId);
                    tempcoefficient.coefficient += temp.coefficient;
                }
                videoCoefficients.Add(tempcoefficient);
            }

            if (historique.Count() == 0) {
                ViewBag.HasHistorique = false;
            } else {
                ViewBag.HasHistorique = true;
            }

            videoCoefficients = videoCoefficients.OrderByDescending(v => v.coefficient).ToList();
            List<int> RecommendedVideos = new List<int>();
            for (int v = 0; v < videoCoefficients.Count(); v++) {
                bool isOk = true;
                foreach (Video vid in historique) {
                    if (vid.VideoId == videoCoefficients[v].id)
                        isOk = false;
                }
                if (isOk)
                    RecommendedVideos.Add(videoCoefficients[v].id);
            }
            var query = videos.Where(c => RecommendedVideos.Contains(c.VideoId)).Take(5).AsEnumerable();

            if (query.Count() == 0) {
                ViewBag.MoreToSee = false;
            } else {
                ViewBag.MoreToSee = true;
            }

            return PartialView("~/Views/Shared/_Recommended.cshtml", query);
        }
    }
}