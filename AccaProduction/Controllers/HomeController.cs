using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccaProduction.Models;
using AccaProduction.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AccaProduction.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccaCandidatesContext _context;

        public HomeController(AccaCandidatesContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {



            Dashboard db = new Dashboard()
            {
                BrojKandidata = _context.Kandidat.Count(),
                BrojIspita = _context.Ispit.Select(i => i.Name.Distinct()).Count(),
                UkupanBrojPolaganja = _context.Polaganja.Count(),
                BrojUspesnihPolaganja = _context.Polaganja.Where(p => p.StatusId == 3).Count(),
                KandidataSrb = _context.Kandidat.Where(k => k.Drzava == "Srbija").Count(),
                KandidataMNE = _context.Kandidat.Where(k => k.Drzava == "Crna Gora").Count(),
                KandidataBH = _context.Kandidat.Where(k => k.Drzava == "Bosna i Hercegovina").Count(),
                KandidataAssurance = _context.Kandidat.Where(k => k.Odeljenje == "Assurance").Count(),
                KandidataTAS = _context.Kandidat.Where(k => k.Odeljenje == "TAS").Count(),
                KandidataOstalo = _context.Kandidat.Where(k=>!(k.Odeljenje=="TAS"||k.Odeljenje=="Assurance")).Count()


            };

            var candidatsPerCountries = _context.Kandidat.GroupBy(m => m.Drzava);

            ViewData["CPCLabels"] = candidatsPerCountries.Select(l => l.Key).ToArray();
            ViewData["CPCCount"] = candidatsPerCountries.Select(l => l.Count()).ToArray();

            ViewData["procenatUspesnosti"] = ((double)db.BrojUspesnihPolaganja / db.UkupanBrojPolaganja).ToString("##.00%");

            ViewData["Labels"] = _context.Rok.Select(r=>r.NazivRoka).ToArray();
            ViewData["Values"] = _context.Polaganja.GroupBy(p=>p.RokId).Select(m=> m.Count() ).ToArray();
            ViewData["PassedCount"] = _context.Polaganja.Where(p => p.StatusId == 3 || p.StatusId == 7).GroupBy(p => p.RokId).Select(m => m.Count()).ToArray();
 
            return View(db);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
