using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.Data_Models
{
    public class VideoUtilisateur
    {
        public int VideoID { get; set; }
        public Video Video { get; set; }

        public int UtilisateurID { get; set; }
        public Utilisateur Utilisateur { get; set; }
    }
}