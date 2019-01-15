using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccaProduction.Models
{
    public partial class StatusPrijave
    {
        public StatusPrijave()
        {
            Polaganja = new HashSet<Polaganja>();
        }

        public int Id { get; set; }

        [Display(Name = "Status Prijave")]
        public string StatusName { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
