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

        [StringLength(40, MinimumLength = 5, ErrorMessage = "The channel's name needs to be between 5 and 40 characters.")]
        [DisplayName("Channel name")]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [MaxLength(180)]
        [DisplayName("Channel description")]
        [RegularExpression(@"(\S*\s){2,}(\S+){1}", ErrorMessage = "The description needs to contain at least 3 words.")]
        public string Description { get; set; }

        [ForeignKey("Utilisateur")]
        public int Utilisateur_FK { get; set; }

        [ForeignKey("Utilisateur_FK")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Required]
        public Categorie Categorie_Chaine { get; set; }

        public virtual ICollection<Video> Videos { get; set; }

        [MaxLength(1050)]
        [RegularExpression("(([A-Za-z0-9_ ]{2,20},){0,49}([A-Za-z0-9_ ]{2,20})){0,1}", ErrorMessage ="The tags need to be 2 to 20 characters, separated by commas. The maximum amount of tags is 50.")]
        public string Tags_Chaine { get; set; }

        [MaxLength(1050)]
        public string AvatarPath { get; set; }
    }
}