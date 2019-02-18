using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.WriteModel
{
    public class KandidatWriteModel
    {

        [Display(Name ="ACCA Broj")]
        public int Id { get; set; }

        [Display(Name ="Ime i prezime")]
        public string FullName { get; set; }

        [Display (Name = "Odeljenje")]
        public string Department { get; set; }

        public string Email { get; set; }

        [Display(Name = "Broj položenih ispita")]
        public int PassedExamCount { get; set; }

    }
}
