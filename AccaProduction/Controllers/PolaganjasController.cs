using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccaProduction.Data;
using AccaProduction.Models;
using AccaProduction.Repository;
using AccaProduction.Utils;
using AccaProduction.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static AccaProduction.ViewModels.PolaganjasView;

namespace AccaProduction.Controllers
{
    [Authorize]
    public class PolaganjasController : Controller
    {
        private readonly AccaCandidatesContext _context;
        private readonly IPolaganjaRepository _polaganja;
        private readonly IKandidatRepository _kandidat;
        private readonly IIspitRepository _ispit;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PolaganjasController(AccaCandidatesContext context, IPolaganjaRepository polaganjaRepository, IKandidatRepository kandidat, IIspitRepository ispit, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _polaganja = polaganjaRepository;
            _kandidat = kandidat;
            _ispit = ispit;
            _hostingEnvironment = hostingEnvironment;
        }

        //GET: Index
        public async Task<IActionResult> Index(string firstAttribute = null, string option = null, string search = null)
        {
            PolaganjasView polaganjasView = new PolaganjasView()
            {                
                Statusi = _context.StatusPrijave.ToList()
            };

            
            if (!User.IsInRole(SD.AdminEndUser))
            {
                int kandidatID = await _kandidat.GetIdByEmail(User.Identity.Name);
                polaganjasView.NonProcessed = await _polaganja.GetNonProcessedRequests(kandidatID);
                polaganjasView.Processed = await _polaganja.GetProcessedRequests(kandidatID);
            }
            else
            {
                polaganjasView.NonProcessed = await _polaganja.GetNonProcessedRequests(firstAttribute,option,search);
                polaganjasView.Processed = await _polaganja.GetProcessedRequests(firstAttribute,option,search);
            }

            return View(polaganjasView);
        }

        //GET: Create
        public async Task<IActionResult> Create( int? ispitID, int? kandidatID)
        {
            if ( (kandidatID==null) || (ispitID==null))
            {
                return NotFound();
            }

            var kandidat = await _kandidat.GetKandidat((int)kandidatID);
            var ispit = await _ispit.GetIspit((int)ispitID);

            if (kandidat==null || ispit==null)
            {
                return NotFound();
            }

            KandidatsAndExams kandidatsAndExams = new KandidatsAndExams()
            {
                Kandidat = kandidat,
                Ispit = ispit,
                Ispits = await _context.Ispit.Select(i => i).ToListAsync(),
                Roks = await _context.Rok.Where(r => r.ActiveStatus).Select(i => i).ToListAsync(),
                BrojPolaganja = await _polaganja.GetExamTakeNumber(ispit.Id,kandidat.IdAccaNumber),
                ExamTakesYTD = await _polaganja.GetExamTakeYTD(kandidat.IdAccaNumber),
                NewPolaganje = new Polaganja()
                {
                    IspitId=ispit.Id,
                    KandidatId=kandidat.IdAccaNumber
                }
                
            };

            return View(kandidatsAndExams);

        }

        //POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KandidatsAndExams model)
        {
            KandidatsAndExams ke = model;
            Polaganja prijava = model.NewPolaganje;

            prijava.StatusId = 1;
            prijava.RequestDate = DateTime.Now;

            _context.Add(prijava);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Kandidats", new { id = model.NewPolaganje.KandidatId });
        }

        //GET Update
        public async Task<IActionResult> Update(int? polaganjeID)
        {
            if (polaganjeID==null)
            {
                return NotFound();
            }

            Polaganja polaganje = await _context.Polaganja.Where(p => p.Id == polaganjeID).Include(r=>r.Rok).FirstOrDefaultAsync();

            if (polaganje==null)
            {
                return NotFound();
            }

            ViewData["Statusi"] = await _context.StatusPrijave.ToListAsync();

            return View(polaganje);
        }

        //POST UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Polaganja polaganje)
        {
            Polaganja polaganjeInDB = await _context.Polaganja.Where(p => p.Id == polaganje.Id).FirstOrDefaultAsync();

            if (polaganjeInDB.KandidatId!=polaganje.KandidatId || polaganjeInDB==null)
            {
                return NotFound();
            }

            polaganjeInDB.StatusId = polaganje.StatusId;
            _context.Update(polaganjeInDB);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}