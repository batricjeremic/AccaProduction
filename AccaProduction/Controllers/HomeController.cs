using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AccaProduction.Models;
using AccaProduction.ViewModels;

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
                BrojIspita = _context.Ispit.Select(i=>i.Name.Distinct()).Count(),
                UkupanBrojPolaganja = _context.Polaganja.Count(),
                BrojUspesnihPolaganja = _context.Polaganja.Where(p=>p.StatusId==3).Count()
            };

            ViewData["procenatUspesnosti"] = (double)db.BrojUspesnihPolaganja / db.UkupanBrojPolaganja;
 
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
