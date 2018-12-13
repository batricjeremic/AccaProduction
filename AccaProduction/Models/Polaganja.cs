using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class Polaganja
    {
        public int Id { get; set; }
        public int KandidatId { get; set; }
        public int IspitId { get; set; }
        public int StatusId { get; set; }

        [Display(Name ="Ispitni Rok")]
        public DateTime DatumPolaganja { get; set; }
        public bool Rezultat { get; set; }

        [Display(Name ="Potrebne knjige")]
        public bool PotrebneKnjige { get; set; }

        [Display(Name ="Početak odsustva")]
        public DateTime? StudyLeaveStartDate { get; set; }

        [Display(Name = "Kraj odsustva")]
        public DateTime? StudyLeaveEndDate { get; set; }

        public Ispit Ispit { get; set; }
        public Kandidat Kandidat { get; set; }
        public StatusPrijave Status { get; set; }
    }
}
