using AccaProduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.ViewModels
{
    public class KandidatsAndExams
    {

        public Kandidat Kandidat { get; set; }

        public IEnumerable<Polaganja> PolozeniIspiti { get; set; }
        public IEnumerable<Ispit> NepolozeniIspiti { get; set; }

        public IEnumerable<Ispit> Ispits { get; set; }
        public Ispit Ispit { get; set; } 

        public int BrojPolaganja { get; set; }

        public Polaganja NewPolaganje { get; set; }

        //public List<string> ImenaPolozenihIspita { get; set; }


    }
}
