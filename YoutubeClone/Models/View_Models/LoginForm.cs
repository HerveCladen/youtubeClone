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
        [Required(ErrorMessage = "Cet utilisateur n'existe pas."),
         DisplayName("Nom d'utilisateur:")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mot de passe incorrect."),
         DisplayName("Mot de passe:"),
         DataType(DataType.Password)]
        public string HashPassword { get; set; }
        [DisplayName("Se souvenir de moi:")]
        public bool RemindMe { get; set; }
    }
}