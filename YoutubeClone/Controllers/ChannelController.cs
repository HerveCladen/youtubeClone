using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YoutubeClone.Models.Data_Models;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;

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
            ViewBag.ChaineId = c.ChaineId;
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
            var Chaine = db.Chaines.Find(id);
            if (User.Identity.IsAuthenticated && User.Identity.Name == db.Utilisateurs.Where(c => c.UtilisateurId == db.Chaines.Where(C => C.ChaineId == id).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username) {
                ViewBag.EditOK = true;
            } else {
                ViewBag.EditOK = false;
            }
            return View("~/Views/Channel/ChannelViewer.cshtml", Chaine);
        }

        //Ne retourne aucune chaîne
        [Authorize]
        public ActionResult ChainesUtilisateurs()
        {
            var Chaines = db.Chaines;
            var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("~/Views/Channel/ChainesUtilisateurs.cshtml", Chaines.Where(c => c.Utilisateur_FK == user));
        }

        // GET: Chaines/Create
        public ActionResult Create() {
            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View();
        }

        // POST: Chaines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Description,Utilisateur_FK,Categorie_Chaine,Tags_Chaine")] Chaine chaineCreate, HttpPostedFileBase avatarUP) {
            if (ModelState.IsValid) {
                try {
                    Chaine chaine = new Chaine();
                    chaine.Name = chaineCreate.Name;
                    chaine.Description = chaineCreate.Description;
                    chaine.Utilisateur_FK = chaineCreate.Utilisateur_FK;
                    chaine.Categorie_Chaine = chaineCreate.Categorie_Chaine;
                    chaine.Tags_Chaine = chaineCreate.Tags_Chaine;
                    db.Chaines.Add(chaine);                    
                    if (avatarUP != null)
                    {
                        chaine.AvatarPath = "/Content/Avatars/" + chaine.ChaineId + Path.GetExtension(avatarUP.FileName);
                        avatarUP.SaveAs(Server.MapPath("~") + "Content\\Avatars\\" + chaine.ChaineId + Path.GetExtension(avatarUP.FileName));                        
                    }
                    db.SaveChanges();
                    return RedirectToAction("ChainesUtilisateurs");
                } catch (Exception e) {
                    return View(chaineCreate);
                }
            }
            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            //return Content(avatarUP.FileName);
            return View(chaineCreate);
        }

        // GET: Chaines/Edit/5
        [Authorize]
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chaine chaine = db.Chaines.Find(id);
            if (chaine == null) {
                return HttpNotFound();
            }

            var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("~/Views/Channel/Edit.cshtml",chaine);
        }

        // POST: Chaines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ChaineId,Name,Description,Utilisateur_FK,Categorie_Chaine,Tags_Chaine")] Chaine chaineEdit, HttpPostedFileBase avatarUP) {
            if (ModelState.IsValid) {
                if (User.Identity.Name == db.Utilisateurs.Where(c => c.UtilisateurId == db.Chaines.Where(C => C.ChaineId == chaineEdit.ChaineId).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username) {
                    try {
                        Chaine chaine = new Chaine();
                        chaine.ChaineId = chaineEdit.ChaineId;
                        chaine.Name = chaineEdit.Name;
                        chaine.Description = chaineEdit.Description;
                        chaine.Utilisateur_FK = chaineEdit.Utilisateur_FK;
                        chaine.Categorie_Chaine = chaineEdit.Categorie_Chaine;
                        chaine.Tags_Chaine = chaineEdit.Tags_Chaine;                        
                        if (avatarUP != null) {
                            chaine.AvatarPath = "/Content/Avatars/" + chaine.ChaineId + Path.GetExtension(avatarUP.FileName);
                            try
                            {
                                System.IO.File.Delete(Server.MapPath("~") + chaine.AvatarPath);
                            } catch (Exception e) { }
                            avatarUP.SaveAs(Server.MapPath("~") + "Content\\Avatars\\" + chaine.ChaineId + Path.GetExtension(avatarUP.FileName));                            
                        }
                        db.Entry(chaine).State = EntityState.Modified;
                        db.SaveChanges();
                        var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
                        return View("~/Views/Channel/ChainesUtilisateurs.cshtml", db.Chaines.Where(c => c.Utilisateur_FK == user));                        
                    } catch (Exception e) {
                        return View("~/Views/Channel/Edit.cshtml", chaineEdit);
                    }
                } else {
                    ModelState.AddModelError("Name", "You may not edit a channel owned by someone else");
                }
            }
            ViewBag.Utilisateur_FK = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
            return View("~/Views/Channel/Edit.cshtml",chaineEdit);
        }

        // GET: Chaines/Delete/5
        [Authorize]
        public ActionResult Delete(int? id, string error = "") {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chaine chaine = db.Chaines.Find(id);
            if (chaine == null) {
                return HttpNotFound();
            }
            ViewBag.Error = error;
            return View("~/Views/Channel/Delete.cshtml",chaine);
        }

        // POST: Chaines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id) {
            if (User.Identity.Name == db.Utilisateurs.Where(c => c.UtilisateurId == db.Chaines.Where(C => C.ChaineId == id).FirstOrDefault().Utilisateur_FK).FirstOrDefault().Username 
                ||
                db.Utilisateurs.Where(c => c.Username == User.Identity.Name).FirstOrDefault().IsAdmin
                ) {
                Chaine chaine = db.Chaines.Find(id);
                try
                {
                    System.IO.File.Delete(Server.MapPath("~") + chaine.AvatarPath);
                } catch (Exception e) { }
                var videos = chaine.Videos.ToList();
                foreach (Video v in videos) {
                    foreach (Commentaire c in v.Commentaires.ToList())
                    {
                        db.Commentaires.Remove(c);
                    }
                    db.Videos.Remove(v);
                    try {
                        System.IO.File.Delete(Server.MapPath("~") + v.ThumbnailPath);
                        System.IO.File.Delete(Server.MapPath("~") + v.VideoPath);
                    } catch (Exception e) {
                    }
                }
                db.Chaines.Remove(chaine);
                db.SaveChanges();
            } else {
                return RedirectToAction("Delete", "Channel", new { error = "You may not delete a channel owned by someone else" });
            }
            var user = db.Utilisateurs.Where(C => C.Username == User.Identity.Name).FirstOrDefault().UtilisateurId;
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