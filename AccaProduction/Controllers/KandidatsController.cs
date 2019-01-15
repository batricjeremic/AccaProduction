using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccaProduction.Models;
using AccaProduction.ViewModels;
using Microsoft.AspNetCore.Authorization;
using AccaProduction.Utils;
using AccaProduction.Repository;

namespace AccaProduction.Controllers
{

    [Authorize(Roles = SD.AdminEndUser)]
    public class KandidatsController : Controller
    {
        private readonly AccaCandidatesContext _context;
        private readonly IKandidatRepository _kandidat;
        private readonly IPolaganjaRepository _polaganja;
        private readonly IIspitRepository _ispit;

        public KandidatsController(AccaCandidatesContext context, IKandidatRepository kandidat, IPolaganjaRepository polaganja, IIspitRepository ispit)
        {
            _context = context;
            this._kandidat = kandidat;
            this._polaganja = polaganja;
            this._ispit = ispit;
        }

        // GET: Kandidats
        public async Task<IActionResult> Index(string option = null, string search = null)
        {

            var kandidats = new List<Kandidat>();

            if (search!=null)
            {
                kandidats = await _kandidat.GetFilteredKandidats(option, search);
            }
            else
            {
                kandidats = await _context.Kandidat.ToListAsync();
            }

            

            return View(kandidats);
        }

        // GET: Kandidats/Details/5
        [AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            int? kandidatId = id;

            if (User.IsInRole(SD.AdminEndUser) && kandidatId == null)
            {
                return NotFound();
            }

            if (User.IsInRole(SD.CandidatEndUSer))
            {
                kandidatId = await _kandidat.GetIdByEmail(User.Identity.Name);
            }

            var kandidat = await _kandidat.GetKandidat((int)kandidatId);

            if (kandidat == null)
            {
                return NotFound();
            }

            KandidatsAndExams ke = new KandidatsAndExams
            {
                Kandidat = kandidat,                
                PolozeniIspiti = await _polaganja.GetCompletedExams((int)kandidatId),
                NepolozeniIspiti = await _ispit.GetNepolozeniIspiti((int)kandidatId)
            };

            return View(ke);
        }

        // GET: Kandidats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kandidats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAccaNumber,Ime,Prezime,Email,Drzava,Odeljenje")] Kandidat kandidat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kandidat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kandidat);
        }

        // GET: Kandidats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kandidat = await _kandidat.GetKandidat((int)id);

            if (kandidat == null)
            {
                return NotFound();
            }

            return View(kandidat);
        }

        // POST: Kandidats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAccaNumber,Ime,Prezime,Email,Drzava,Odeljenje")] Kandidat kandidat)
        {
            if (id != kandidat.IdAccaNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kandidat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KandidatExists(kandidat.IdAccaNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kandidat);
        }

        // GET: Kandidats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kandidat = await _context.Kandidat
                .FirstOrDefaultAsync(m => m.IdAccaNumber == id);
            if (kandidat == null)
            {
                return NotFound();
            }

            return View(kandidat);
        }

        // POST: Kandidats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kandidat = await _context.Kandidat.FindAsync(id);
            _context.Kandidat.Remove(kandidat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KandidatExists(int id)
        {
            return _context.Kandidat.Any(e => e.IdAccaNumber == id);
        }
    }
}
