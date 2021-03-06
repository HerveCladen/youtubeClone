﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoutubeClone.Models.View_Models {
    public class VideoEdit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VideoId { get; set; }

        [ForeignKey("Channel")]
        public int Chaine_FK { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 5)]
        [DisplayName("Entrez le nom de la video")]
        public string Name { get; set; }

        [Required]
        [MaxLength(180)]
        [DisplayName("Entrez une description")]
        [RegularExpression(@"(\S*\s){2,}(\S+){1}")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Choisissez la categorie")]
        public Data_Models.Categorie Categorie_Video { get; set; }

        [RegularExpression("(([A-Za-z0-9_ ]{2,20},){0,49}([A-Za-z0-9_ ]{2,20})){0,1}")]
        [MaxLength(1050)]
        public string Tags_Video { get; set; }
        
    }
}