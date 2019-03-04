using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Entrez un nom d'utilisateur valide"),
         DisplayName("Nom d'utilisateur:")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Entrez le bon mot de passe"),
         DisplayName("Mot de passe:"),
         DataType(DataType.Password)]
        public string HashPassword { get; set; }
        [DisplayName("Se souvenir de moi")]
        public bool RemindMe { get; set; }
    }
}