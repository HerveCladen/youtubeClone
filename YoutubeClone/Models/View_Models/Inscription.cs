using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.View_Models
{
    public class Inscription : Profil
    {
        [Range(typeof(bool), "true", "true", ErrorMessage = "Vous devez accepter les termes et conditions d'utilisation du site pour continuer.")] 
        [DisplayName("J'accepte les termes et conditions d'utilisation du site.")]
        public bool IAgree { get; set; }
    }
}