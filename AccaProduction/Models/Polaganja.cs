using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class Polaganja
    {
        public int Id { get; set; }

        [Display(Name = "Acca Broj")]
        public int KandidatId { get; set; }

        [Display(Name = "ID Ispita")]
        public int IspitId { get; set; }

        
        public int StatusId { get; set; }

        [Display(Name = "Rok")]
        public int RokId { get; set; }

        [Display(Name = "Potrebne knjige")]
        public bool PotrebneKnjige { get; set; }

        [Display(Name = "Početak odsustva")]
        public DateTime? StudyLeaveStartDate { get; set; }
        [Display(Name = "Završetak odsustva")]
        public DateTime? StudyLeaveEndDate { get; set; }

        [Required]
        [Display(Name =("Datum Podnošenja Zahteva"))]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime RequestDate { get; set; }

        public Ispit Ispit { get; set; }
        public Kandidat Kandidat { get; set; }
        public Rok Rok { get; set; }
        public StatusPrijave Status { get; set; }
    }
}
