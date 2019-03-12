using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YoutubeClone.Models.Data_Models;

namespace YoutubeClone.Controllers
{
    
    public class HomeController : Controller {
        private YoutubeCloneDb db = new YoutubeCloneDb();
        // GET: Home
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                if (db.Utilisateurs.Where(c => c.Username == User.Identity.Name).Count() == 0) {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult MenuCategories() {
            return PartialView("~/Views/Shared/_MenuCategories.cshtml", Enum.GetValues(typeof(Models.Data_Models.Categorie)).Cast<Models.Data_Models.Categorie>());
        }
    }
}