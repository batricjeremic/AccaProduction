using System;
using System.Collections.Generic;

namespace AccaProduction.Models
{
    public partial class StatusPrijave
    {
        public StatusPrijave()
        {
            Polaganja = new HashSet<Polaganja>();
        }

        public int Id { get; set; }
        public string StatusName { get; set; }

        public ICollection<Polaganja> Polaganja { get; set; }
    }
}
