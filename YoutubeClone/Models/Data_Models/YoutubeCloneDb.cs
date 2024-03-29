﻿using System;
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
        public DbSet<Commentaire> Commentaires { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Utilisateur>().HasMany(p => p.Historique);
            modelBuilder.Entity<Utilisateur>().HasMany(p => p.Commentaires);
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
            Random rnd = new Random();

            Utilisateur u1 = new Utilisateur() { Username = "kidbestcode", Courriel = "David.genois13@gmail.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = true };
            Utilisateur u2 = new Utilisateur() { Username = "ace13", Courriel = "e1671873@cmaisonneuve.qc.ca", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u3 = new Utilisateur() { Username = "jijininja", Courriel = "frandre@videotron.ca", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u4 = new Utilisateur() { Username = "buzz5", Courriel = "a@a.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Utilisateur u5 = new Utilisateur() { Username = "marcamnesia", Courriel = "b@b.com", HashPassword = Profil.Cryptage("patate"), IsAdmin = false };
            Chaine ch1 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Pewdiepie", Description = "Patate Pata  sdasdsa asdas te Patate", Utilisateur = u1 };
            Chaine ch2 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "ReactGaming", Description = "Patate Patat asdasd asdasd dse Patate", Utilisateur = u2 };
            Chaine ch3 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Jacksepticeye", Description = "Patate asdasd asdas Patate Patate", Utilisateur = u4 };
            Chaine ch4 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "SxyHxy", Description = "Patate Patat sdfas asdas nne Patate", Utilisateur = u5 };
            Chaine ch5 = new Chaine() { Categorie_Chaine = Categorie.Fashion, Name = "Molfried", Description = "Patate Ps asdasd asdasd atate Patate", Utilisateur = u4 };
            Video v1 = new Video() { Views = rnd.Next(1, 100), Name = "Potato Patata", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today, Channel = ch3, Likes = 2, Dislikes = 1 };
            Video v2 = new Video() { Views = rnd.Next(1, 100), Name = "Preztel Apocalypse", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-10), Channel = ch3, Likes = 5, Dislikes = 1 };
            Video v3 = new Video() { Views = rnd.Next(1, 100), Name = "Pizza mania", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-15), Channel = ch1, Likes = 2, Dislikes = 3 };
            Video v4 = new Video() { Views = rnd.Next(1, 100), Name = "Patata On a Pazza", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-31), Channel = ch5, Likes = 1, Dislikes = 0 };
            Video v5 = new Video() { Views = rnd.Next(1, 100), Name = "Why are we not rocks", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-62), Channel = ch2, Likes = 4, Dislikes = 1 };
            Video v6 = new Video() { Views = rnd.Next(1, 100), Name = "Why are we not rocks 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-365), Channel = ch2, Likes = 25, Dislikes = 1 };
            Video v7 = new Video() { Views = rnd.Next(1, 100), Name = "high school bully", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-750), Channel = ch4, Likes = 0, Dislikes = 0 };
            Video v8 = new Video() { Views = rnd.Next(1, 100), Name = "Potato Patata 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-2), Channel = ch3, Likes = 2, Dislikes = 5 };
            Video v9 = new Video() { Views = rnd.Next(1, 100), Name = "Preztel Apocalypse 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-160), Channel = ch3, Likes = 3, Dislikes = 2 };
            Video v10 = new Video() { Views = rnd.Next(1, 100), Name = "Pizza mania 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-985), Channel = ch1, Likes = 5, Dislikes = 2 };
            Video v11 = new Video() { Views = rnd.Next(1, 100), Name = "Patata On a Pazza 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-3250), Channel = ch5, Likes = 2, Dislikes = 7 };
            Video v12 = new Video() { Views = rnd.Next(1, 100), Name = "Why are we not rocks 4", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-1), Channel = ch2, Likes = 5, Dislikes = 5 };
            Video v13 = new Video() { Views = rnd.Next(1, 100), Name = "Why are we not rocks 3", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-3), Channel = ch2, Likes = 2, Dislikes = 2 };
            Video v14 = new Video() { Views = rnd.Next(1, 100), Name = "high school bully 2", Description = "This is a description", Categorie_Video = Categorie.React, DatePublished = DateTime.Today.AddDays(-7), Channel = ch4, Likes = 25, Dislikes = 5 };
            Video v15 = new Video() { Views = rnd.Next(1, 100), Name = "video", Description = "This is a description", Categorie_Video = Categorie.Meme, DatePublished = DateTime.Today.AddDays(-7), Channel = ch4, Likes = 15, Dislikes = 5 };

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
            context.Videos.Add(v15);
            context.SaveChanges();
        }
    }
}