using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.View_Models
{
    [CustomValidation(typeof(Inscription), "Valider")]
    public class Inscription : Profil
    {
        [Required, DisplayName("J'accepte les termes et conditions d'utilisation du site.")]
        public bool IAgree { get; set; }
        public static ValidationResult Valider(Inscription i)
        {
            if (!i.IAgree)
            {
                return new ValidationResult("Clique oui.", new[] { "IAgree" });
            }
            else
                return ValidationResult.Success;
        }
    }
}