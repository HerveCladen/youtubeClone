using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.Data_Models {
    public class Chaine
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChaineId { get; set; }

        [StringLength(40, MinimumLength = 5, ErrorMessage = "Le nom de la chaîne doit être entre 5 et 40 caractères.")]
        [DisplayName("Nom de la chaine:")]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(180)]
        [DisplayName("Description de la chaine:")]
        [RegularExpression(@"(\S*\s){2,}(\S+){1}", ErrorMessage = "La description doit contenir au moins 3 mots.")]
        public string Description { get; set; }

        [ForeignKey("Utilisateur")]
        public int Utilisateur_FK { get; set; }

        [ForeignKey("Utilisateur_FK")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Required]
        public Categorie Categorie_Chaine { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        [MaxLength(1050)]
        [RegularExpression("(([A-Za-z0-9_ ]{2,20},){0,49}([A-Za-z0-9_ ]{2,20})){0,1}", ErrorMessage ="Les tags doivent être de 2 à 20 caractères et doivent être séparés par des virgules, et il y a un max de 50 tags.")]
        public string Tags_Chaine { get; set; }
    }
}