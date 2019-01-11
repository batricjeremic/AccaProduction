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

        public KandidatsController(AccaCandidatesContext context)
        {
            _context = context;
        }

        // GET: Kandidats
        public async Task<IActionResult> Index(string option = null, string search = null)
        {

            var kandidats = await _context.Kandidat.ToListAsync();


            //this code block should be replaced with dynamic query
            if (option == "Email" && search != null)
            {
                kandidats = kandidats.Where(u => u.Email.ToLower().Contains(search.ToLower())).ToList();
            }
            else
            {
                if (option == "Ime" && search != null)
                {
                    kandidats = kandidats.Where(u => u.Ime.ToLower().Contains(search.ToLower())
                            || u.Prezime.ToLower().Contains(search.ToLower())
                    ).ToList();
                }
                else
                {
                    if (option == "Odeljenje" && search != null)
                    {
                        kandidats = kandidats.Where(u => u.Odeljenje.ToLower().Contains(search.ToLower())).ToList();
                    }
                }
            }

            return View(kandidats);
        }

        // GET: Kandidats/Details/5
        public async Task<IActionResult> Details(int? id)
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

            KandidatsAndExams ke = new KandidatsAndExams
            {
                Kandidat = kandidat,
                PolozeniIspiti = _context.Polaganja.Where(p => (p.KandidatId == kandidat.IdAccaNumber) && (p.StatusId == 3 || p.StatusId == 7)).Include(i => i.Ispit).Include(r => r.Rok).OrderBy(i => i.IspitId).ToList(),
                NepolozeniIspiti = _context.Ispit.Include(p => p.Polaganja).Where(i => i.Polaganja.All(k => !(k.KandidatId == id) || (k.StatusId != 3 && k.StatusId != 7))).OrderBy(i => i.Id)
            };

            return View(ke);
        }

        // GET: Kandidats/Detalji/email
        [AllowAnonymous]
        [Authorize(Roles = SD.CandidatEndUSer)]
        public async Task<IActionResult> Detalji(string email)
        {
            if (email == null)
            {
                return NotFound();
            }

            int id = await _context.Kandidat.Where(k => k.Email == email).Select(i => i.IdAccaNumber).FirstOrDefaultAsync();

            var kandidat = await _context.Kandidat
                .FirstOrDefaultAsync(m => m.IdAccaNumber == id);
            if (kandidat == null)
            {
                return NotFound();
            }

            KandidatsAndExams ke = new KandidatsAndExams
            {
                Kandidat = kandidat,
                PolozeniIspiti = _context.Polaganja.Where(p => (p.KandidatId == kandidat.IdAccaNumber) && (p.StatusId == 3 || p.StatusId == 7)).Include(i => i.Ispit).Include(r => r.Rok).OrderBy(i => i.IspitId).ToList(),
                NepolozeniIspiti = _context.Ispit.Include(p => p.Polaganja).Where(i => i.Polaganja.All(k => !(k.KandidatId == id) || k.StatusId != 3 || k.StatusId != 7)).OrderBy(i => i.Id)
            };

            return View("Details", ke);
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

            var kandidat = await _context.Kandidat.FindAsync(id);
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
