using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace YoutubeClone.Models.View_Models
{
    public class Profil
    {
        [Required(ErrorMessage = "Veuillez entrer un nom d'utilisateur.")]
        [DisplayName("Nom d'utilisateur:")]
        [StringLength(18, MinimumLength = 5, ErrorMessage = "Le nom d'utilisateur doit être en 5 et 18 caractères.")]
        [RegularExpression("^[a-zA-Z0-9_.-]*")]
        public string UserName { get; set; }

        [DisplayName("Mot de passe:")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Veuillez entrer un mot de passe.")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Votre mot de passe doit contenir au moins une lettre majuscule, une miniscule et un chiffre.")]
        [StringLength(80, MinimumLength = 6, ErrorMessage = "Le mot de passe doit être entre 6 et 80 caractères.")]
        public string HashPassword { get; set; }

        [DisplayName("Vérifiez votre mot de passe:")]
        [DataType(DataType.Password)]
        [Compare("HashPassword", ErrorMessage = "Les deux champs ne sont pas identiques.")]
        [StringLength(80, MinimumLength = 6, ErrorMessage = "Le mot de passe doit être entre 6 et 80 caractères.")]
        public string VerifiedPass { get; set; }

        [Required(ErrorMessage = "Veuillez entrer une adresse courriel.")]
        [DisplayName("Courriel:")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Veuillez entrer une adresse courriel valide.")]
        [MaxLength(50)]
        public string Email { get; set; }

        [DisplayName("Confirmez votre courriel:")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Veuillez entrer une adresse courriel valide.")]
        [Compare("Email", ErrorMessage = "Les deux champs ne sont pas identiques.")]
        [MaxLength(50)]
        public string VerifiedEmail { get; set; }

        public static string Cryptage(string pass)
        {
            byte[] passEncoder = new UTF8Encoding().GetBytes(pass);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passEncoder);
            string encoder = BitConverter.ToString(hash);

            return encoder;
        }
    }
}