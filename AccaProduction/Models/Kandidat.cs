using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class Kandidat
    {
        public Kandidat()
        {
            Polaganja = new HashSet<Polaganja>();
        }

        [Display(Name ="Acca Broj")]
        public int IdAccaNumber { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }

        [Display(Name ="E-mail")]
        public string Email { get; set; }

        [Display(Name ="Država")]
        public string Drzava { get; set; }
        public string Odeljenje { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
