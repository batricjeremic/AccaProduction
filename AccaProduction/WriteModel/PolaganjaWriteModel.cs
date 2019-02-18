using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.WriteModel
{
    public class PolaganjaWriteModel
    {
        public int Id { get; set; }

        public int IspitId { get; set; }

        [Display(Name ="Oznaka")]
        public string NewCode { get; set; }
        [Display(Name = "Naziv ispita")]
        public string IspitName { get; set; }

        [Display(Name ="Stara oznaka")]
        public string OldCode { get; set; }

        public int KandidatId { get; set; }

        [Display(Name = "Ime i prezime")]
        public string KandidatFullName { get; set; }

        public string Rok { get; set; }
        public string Status { get; set; }
    }
}
