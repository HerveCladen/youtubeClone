using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YoutubeClone.Models.Data_Models;

namespace YoutubeClone_Bruce.Controllers
{
    public class UtilisateursController : Controller
    {
        private YoutubeCloneDb db = new YoutubeCloneDb();

        // GET: Utilisateurs
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Utilisateurs.ToList());
        }

        // GET: Utilisateurs/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, string error = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur utilisateur = db.Utilisateurs.FirstOrDefault(model => model.UtilisateurId == id);
            if (utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = error;
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.Identity.Name == db.Utilisateurs.Find(id).Username
                ||
                db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin
                )
            {
                Utilisateur utilisateur = db.Utilisateurs.Find(id);
                var chaines = utilisateur.Chaines.ToList();
                foreach (Chaine c in chaines)
                {
                    var videos = c.Videos.ToList();
                    foreach (Video v in videos)
                    {
                        db.Videos.Remove(v);
                        try
                        {
                            System.IO.File.Delete(Server.MapPath("~") + v.ThumbnailPath);
                            System.IO.File.Delete(Server.MapPath("~") + v.VideoPath);
                        }
                        catch (Exception e)
                        {
                            e.StackTrace.ToString();
                        }
                    }
                    db.Chaines.Remove(c);
                }
                if (!db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin)
                    FormsAuthentication.SignOut();
                db.Utilisateurs.Remove(utilisateur);
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("Delete", "Utilisateurs", new { error = "You may not delete someone else's account" });
            }
            return RedirectToAction("Index", "Home", null);
        }

        [Authorize]
        public ActionResult Historique()
        {
            Utilisateur user = db.Utilisateurs.First(c => c.Username == User.Identity.Name);
            return View("~/Views/Videos/Historique.cshtml", user.Historique);
        }

        [Authorize]
        public ActionResult VideHistorique()
        {
            Utilisateur user = db.Utilisateurs.First(c => c.Username == User.Identity.Name);
            user.Historique.Clear();
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return View("~/Views/Videos/Historique.cshtml", user.Historique);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Admin()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin)
                {

                    ViewBag.Videos = db.Videos.AsEnumerable();
                    ViewBag.Chaines = db.Chaines.AsEnumerable();
                    ViewBag.Utilisateurs = db.Utilisateurs.AsEnumerable();

                    return View("~/Views/Utilisateurs/Admin.cshtml");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AdminMenuOption()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin)
                {
                    return PartialView("~/Views/Shared/_AdminMenuOption.cshtml");
                }
                else
                {
                    return Content("");
                }
            }
            else
            {
                return Content("");
            }
        }

    }
}
