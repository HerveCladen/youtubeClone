using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models
{
    [CustomValidation(typeof(LoginForm), "Valider")]
    public class LoginForm
    {
        [Required, DisplayName("Nom d'utilisateur:")]
        public string UserName { get; set; }
        [Required, DisplayName("Mot de passe:"), DataType(DataType.Password)]
        public string HashPassword { get; set; }
        [DisplayName("Se souvenir de moi")]
        public bool RemindMe { get; set; }

    }
}