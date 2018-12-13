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
        public string OldCode { get; set; }
        public string NewCode { get; set; }

        [Display(Name="Naziv Ispita")]
        public string Name { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
