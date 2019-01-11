using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccaProduction.Data;
using AccaProduction.Models;
using AccaProduction.Utils;
using AccaProduction.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccaProduction.Controllers
{
    [Authorize]
    public class PolaganjasController : Controller
    {
        private readonly AccaCandidatesContext _context;

        public PolaganjasController(AccaCandidatesContext db)
        {
            _context = db;
        }

        //GET: Index
        public IActionResult Index()
        {
            PolaganjasView polaganjasView = new PolaganjasView()
            {
                NonProcessed = _context.Polaganja.Include(p => p.Kandidat).Include(p => p.Ispit).Include(p => p.Status).Include(p=>p.Rok).Where(p => p.StatusId < 3),
                Processed = _context.Polaganja.Include(p => p.Kandidat).Include(p => p.Ispit).Include(p => p.Status).Include(p => p.Rok).Where(p => p.StatusId >= 3),
                Statusi = _context.StatusPrijave.ToList()
            };

            if (!User.IsInRole(SD.AdminEndUser))
            {
                int kandidatID = _context.Kandidat.Where(k => k.Email == User.Identities.FirstOrDefault().Name).Select(i => i.IdAccaNumber).FirstOrDefault();

                polaganjasView.NonProcessed = polaganjasView.NonProcessed.Where(k => k.KandidatId == kandidatID);
                polaganjasView.Processed = polaganjasView.Processed.Where(k => k.KandidatId == kandidatID);
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

            ViewData["Locked"] = true;

            var kandidat = _context.Kandidat.Where(k => k.IdAccaNumber == kandidatID).FirstOrDefault();
            var ispit = _context.Ispit.Where(i => i.Id == ispitID).FirstOrDefault();
            var ispits = _context.Ispit.Select(i => i).ToList();
            var roks = await _context.Rok.Where(r=>r.ActiveStatus).Select(i => i).ToListAsync();

            int? brojPolaganja = _context.Polaganja.Where(p => p.IspitId == ispitID && p.KandidatId == kandidatID).Count();

            if (kandidat==null || ispits==null)
            {
                return NotFound();
            }

            KandidatsAndExams kandidatsAndExams = new KandidatsAndExams()
            {
                Kandidat = kandidat,
                Ispits = ispits,
                Roks = roks,
                Ispit = ispit,
                BrojPolaganja = brojPolaganja ?? 0,
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
            //prijava.RokId = ke.Rok.Id;
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

            Polaganja polaganje = await _context.Polaganja.Where(p => p.Id == polaganjeID).FirstOrDefaultAsync();

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