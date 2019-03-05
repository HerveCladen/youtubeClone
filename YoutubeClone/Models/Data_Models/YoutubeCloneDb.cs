using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using YoutubeClone.Models.View_Models;

namespace YoutubeClone.Models.Data_Models
{
    public class YoutubeCloneDb : DbContext
    {
        public YoutubeCloneDb() : base("cnx")
        {
            Database.SetInitializer(new YoutubeCloneInitializer());
        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Chaine> Chaines { get; set; }
        //public DbSet<VideoUtilisateur> VideoUtilisateur { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Utilisateur>().HasMany(p => p.Historique)/*.WithRequired()*/;
        }
    }

    public class YoutubeCloneInitializer :
        DropCreateDatabaseIfModelChanges<YoutubeCloneDb>
    {
        public YoutubeCloneInitializer() : base() { }
        public override void InitializeDatabase(YoutubeCloneDb context)
        {
            base.InitializeDatabase(context);

        }
        protected override void Seed(YoutubeCloneDb context)
        {

            Utilisateur u1 = new Utilisateur() { Username = "kidbestcode", Courriel = "David.genois13@gmail.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u2 = new Utilisateur() { Username = "ace13", Courriel = "e1671873@cmaisonneuve.qc.ca", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u3 = new Utilisateur() { Username = "jijininja", Courriel = "frandre@videotron.ca", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u4 = new Utilisateur() { Username = "buzz5", Courriel = "a@a.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u5 = new Utilisateur() { Username = "marcamnesia", Courriel = "b@b.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Chaine ch1 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Pewdiepie", Description = "Patate Pata  sdasdsa asdas te Patate", Utilisateur = u1 };
            Chaine ch2 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "ReactGaming", Description = "Patate Patat asdasd asdasd dse Patate", Utilisateur = u2 };
            Chaine ch3 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Jacksepticeye", Description = "Patate asdasd asdas Patate Patate", Utilisateur = u4 };
            Chaine ch4 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "SxyHxy", Description = "Patate Patat sdfas asdas nne Patate", Utilisateur = u5 };
            Chaine ch5 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Molfried", Description = "Patate Ps asdasd asdasd atate Patate", Utilisateur = u4 };
            Video v1 = new Video() { Views = new Random().Next(1, 100), Name = "Potato Patata", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch3 };
            Video v2 = new Video() { Views = new Random().Next(1, 100), Name = "Preztel Apocalypse", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch3 };
            Video v3 = new Video() { Views = new Random().Next(1, 100), Name = "Pizza mania", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch1 };
            Video v4 = new Video() { Views = new Random().Next(1, 100), Name = "Patata On a Pazza", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch5 };
            Video v5 = new Video() { Views = new Random().Next(1, 100), Name = "Why are we not rocks", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch2 };
            Video v6 = new Video() { Views = new Random().Next(1, 100), Name = "Why are we not rocks 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch2 };
            Video v7 = new Video() { Views = new Random().Next(1, 100), Name = "high school bully", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch4 };
            Video v8 = new Video() { Views = new Random().Next(1, 100), Name = "Potato Patata 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch3 };
            Video v9 = new Video() { Views = new Random().Next(1, 100), Name = "Preztel Apocalypse 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch3 };
            Video v10 = new Video() { Views = new Random().Next(1, 100), Name = "Pizza mania 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch1 };
            Video v11 = new Video() { Views = new Random().Next(1, 100), Name = "Patata On a Pazza 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch5 };
            Video v12 = new Video() { Views = new Random().Next(1, 100), Name = "Why are we not rocks 4", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch2 };
            Video v13 = new Video() { Views = new Random().Next(1, 100), Name = "Why are we not rocks 3", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch2 };
            Video v14 = new Video() { Views = new Random().Next(1, 100), Name = "high school bully 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch4 };

            context.Utilisateurs.Add(u1);
            context.Utilisateurs.Add(u2);
            context.Utilisateurs.Add(u3);
            context.Utilisateurs.Add(u4);
            context.Utilisateurs.Add(u5);
            context.Chaines.Add(ch1);
            context.Chaines.Add(ch2);
            context.Chaines.Add(ch3);
            context.Chaines.Add(ch4);
            context.Chaines.Add(ch5);
            context.Videos.Add(v1);
            context.Videos.Add(v2);
            context.Videos.Add(v3);
            context.Videos.Add(v4);
            context.Videos.Add(v5);
            context.Videos.Add(v6);
            context.Videos.Add(v7);
            context.Videos.Add(v8);
            context.Videos.Add(v9);
            context.Videos.Add(v10);
            context.Videos.Add(v11);
            context.Videos.Add(v12);
            context.Videos.Add(v13);
            context.Videos.Add(v14);
            context.SaveChanges();
        }
    }
}