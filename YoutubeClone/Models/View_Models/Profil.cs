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
        [DisplayName("Nom")]
        public string UserName { get; set; }
        [DisplayName("Mot de passe:"), DataType(DataType.Password)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        public string HashPassword { get; set; }
        [DisplayName("Verifier le mot de passe:"), DataType(DataType.Password)]
        [Compare("HashPassword")]
        public string VerifiedPass { get; set; }
        [Required, DisplayName("Courriel"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DisplayName("Courriel"), DataType(DataType.EmailAddress)]
        [Compare("Email")]
        public string VerifiedEmail { get; set; }
        protected String Cryptage(String pass)
        {
            byte[] passEncoder = new UTF8Encoding().GetBytes(pass);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(passEncoder);
            string encoder = BitConverter.ToString(hash);

            return encoder;
        }

        public string Encodage(string pass)
        {
            return Cryptage(pass);
        }
    }
}