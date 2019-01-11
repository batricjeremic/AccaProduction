using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class Rok
    {
        public Rok()
        {
            Polaganja = new HashSet<Polaganja>();
        }

        public int Id { get; set; }
        [Display(Name = "Rok")]
        public string NazivRoka { get; set; }
        [Display(Name = "Aktivan za prijavu")]
        public bool ActiveStatus { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
