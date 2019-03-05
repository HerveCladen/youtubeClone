using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.Data_Models
{
    public class Utilisateur
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UtilisateurId { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 5)]
        [Index(IsUnique = true)]
        [RegularExpression("^[a-zA-Z0-9_.-]*")]//Might cause issues
        public string Username { get; set; }

        [EmailAddress]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Courriel { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 6)]
        //VERIF DANS LE VIEW MODAL DANS LA CREATION
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]//Might cause issues
        [Display(Name = "Password")]

        public string HashPassword { get; set; }

        public virtual ICollection<Chaine> Chaines { get; set; }

        //Sera sauver en ordre chronologique, pas besoin de save la date
        //public ICollection<VideoUtilisateur> Historique { get; set; }
        public virtual ICollection<Video> Historique { get; set; }

        public bool IsAdmin { get; set; }   
        
        public Utilisateur()
        {
            //Historique = new List<VideoUtilisateur>();
            Historique = new List<Video>();
        }
    }
}