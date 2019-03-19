using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.Data_Models {
    public enum Categorie {Jeux, Sport, Fashion, Review, React, Meme}
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VideoId { get; set; }

        [DisplayName("Views")]
        public int Views { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 5)]
        [DisplayName("Video title")]
        public string Name { get; set; }

        [Required]
        [MaxLength(180)]
        [DisplayName("Description")]
        [RegularExpression(@"(\S*\s){2,}(\S+){1}")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Categorie")]
        public Categorie Categorie_Video { get; set; }

        [Required]
        [DisplayName("Date de publication")]
        public DateTime DatePublished { get; set; }

        [ForeignKey("Channel")]
        public int Chaine_FK { get; set; }

        [ForeignKey("Chaine_FK")]
        public virtual Chaine Channel { get; set; }

        [DisplayName("Tags")]
        [RegularExpression("(([A-Za-z0-9_ ]{2,20},){0,49}([A-Za-z0-9_ ]{2,20})){0,1}")]
        [MaxLength(1050)]
        public string Tags_Video { get; set; }

        [MaxLength(300)]
        public string VideoPath { get; set; }

        [MaxLength(300)]
        public string ThumbnailPath { get; set; }
        
        public virtual ICollection<Commentaire> Commentaires { get; set; }
        
        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public Video() {
            DatePublished = DateTime.Now;
        }
    }
}