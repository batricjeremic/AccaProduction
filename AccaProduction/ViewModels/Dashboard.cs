using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.ViewModels
{
    public class Dashboard
    {

        public int BrojKandidata { get; set; }
        public int BrojIspita { get; set; }
        public int UkupanBrojPolaganja { get; set; }
        public int BrojUspesnihPolaganja { get; set; }


        public int KandidataSrb { get; set; }
        public int KandidataBH { get; set; }
        public int KandidataMNE { get; set; }

        public int KandidataAssurance { get; set; }
        public int KandidataTAS { get; set; }
        public int KandidataOstalo { get; set; }


    }
}
