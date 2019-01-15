using AccaProduction.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.ViewModels
{
    public class PolaganjasView
    {

        public IEnumerable<Polaganja> Processed { get; set; }



        public IEnumerable<NonProcessedExams> NonProcessed { get; set; }

        public IEnumerable<StatusPrijave> Statusi { get; set; }

        public int StatusID { get; set; }
        public int PolaganjeID { get; set; }

        public class NonProcessedExams : Polaganja
        {
            [Display(Name = "Redni broj polaganja")]
            public int TakeNumber { get; set; }

            public NonProcessedExams(Polaganja polaganje)
            {
                this.Id = polaganje.Id;
                this.Ispit = polaganje.Ispit;
                this.IspitId = polaganje.IspitId;
                this.Kandidat = polaganje.Kandidat;
                this.KandidatId = polaganje.KandidatId;
                this.PotrebneKnjige = polaganje.PotrebneKnjige;
                this.RequestDate = polaganje.RequestDate;
                this.Rok = polaganje.Rok;
                this.RokId = polaganje.RokId;
                this.Status = polaganje.Status;
                this.StatusId = polaganje.StatusId;
                this.StudyLeaveEndDate = polaganje.StudyLeaveEndDate;
                this.StudyLeaveStartDate = polaganje.StudyLeaveStartDate;
            }
        }


    }
}
