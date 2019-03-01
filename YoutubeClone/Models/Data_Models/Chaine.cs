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
        [MinLength(5), MaxLength(40),DisplayName("Entre le nom de la chaine"), Index(IsUnique = true)]
        public string Name { get; set; }
        [Required, MaxLength(180), DisplayName("Entre la description de la chaine")]
        [RegularExpression(@"(\S*\s){2,}(\S+){1}")]
        public string Description { get; set; }
        [ForeignKey("Utilisateur")]
        public int Utilisateur_FK { get; set; }
        [ForeignKey("Utilisateur_FK")]
        public virtual Utilisateur Utilisateur { get; set; }
        [Required]
        public Categorie Categorie_Chaine { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
        [MaxLength(1050)]
        [RegularExpression("(([A-Za-z0-9_ ]{2,20},){0,49}([A-Za-z0-9_ ]{2,20})){0,1}")]
        public string Tags_Chaine { get; set; }
    }
}