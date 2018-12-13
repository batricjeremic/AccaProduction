using AccaProduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.ViewModels
{
    public class PolaganjasView
    {

        public IEnumerable<Polaganja> Processed { get; set; }
        public IEnumerable<Polaganja> NonProcessed { get; set; }

        public IEnumerable<StatusPrijave> Statusi { get; set; }

        public int StatusID { get; set; }
        public int PolaganjeID { get; set; }

    }
}
