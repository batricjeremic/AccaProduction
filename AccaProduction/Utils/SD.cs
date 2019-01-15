using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.Utils
{
    public class SD
    {
        public const string AdminEndUser = "Admin";
        public const string CandidatEndUSer = "Kandidat";

        public enum StatusPrijave { ZahtevPodnet=1,PrijavaProsledjena,IspitPolozen, NedovoljnoBodova,KandidatOdsutan,ZahtevOdbijen,OslobodjenPolaganja }
    }
}
