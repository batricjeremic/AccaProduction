using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class Ispit
    {
        public Ispit()
        {
            Polaganja = new HashSet<Polaganja>();
        }

        public int Id { get; set; }

        [Display(Name = "Oznaka")]
        public string OldCode { get; set; }

        [Display(Name = "Nova oznaka")]
        public string NewCode { get; set; }

        [Display(Name = "Naziv ispita")]
        public string Name { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
