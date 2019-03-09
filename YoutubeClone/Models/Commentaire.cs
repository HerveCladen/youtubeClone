using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.Data_Models
{
    public class Commentaire
    {
        [Key]
        public string CommentaireId { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Contenu { get; set; }

        [Required]
        [DisplayName("Date de publication")]
        public DateTime DatePublication { get; set; }

        [ForeignKey("Auteur")]
        public int Chaine_FK { get; set; }

        [ForeignKey("Chaine_FK")]
        public virtual Chaine Auteur { get; set; }

        [ForeignKey("Video")]
        public int Video_FK { get; set; }

        [ForeignKey("Video_FK")]
        public virtual Video Video { get; set; }

        public virtual ICollection<Commentaire> Reponses { get; set; }

        public Commentaire()
        {
            DatePublication = DateTime.Now;
        }
    }
}