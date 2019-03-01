using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YoutubeClone.Controllers
{
    
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuCategories() {
            return PartialView("~/Views/Shared/_MenuCategories.cshtml", Enum.GetValues(typeof(Models.Data_Models.Categorie)).Cast<Models.Data_Models.Categorie>());
        }
    }
}