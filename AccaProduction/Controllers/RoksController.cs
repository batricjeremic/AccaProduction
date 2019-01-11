using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccaProduction.Models;
using Microsoft.AspNetCore.Authorization;
using AccaProduction.Utils;

namespace AccaProduction.Controllers
{
    [Authorize (Roles = SD.AdminEndUser)]
    public class RoksController : Controller
    {
        private readonly AccaCandidatesContext _context;

        public RoksController(AccaCandidatesContext context)
        {
            _context = context;
        }

        // GET: Roks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rok.ToListAsync());
        }

        // GET: Roks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rok = await _context.Rok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rok == null)
            {
                return NotFound();
            }

            return View(rok);
        }

        // GET: Roks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NazivRoka,ActiveStatus")] Rok rok)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rok);
        }

        // GET: Roks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rok = await _context.Rok.FindAsync(id);
            if (rok == null)
            {
                return NotFound();
            }
            return View(rok);
        }

        // POST: Roks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazivRoka,ActiveStatus")] Rok rok)
        {
            if (id != rok.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RokExists(rok.Id))
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
            return View(rok);
        }

        // GET: Roks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rok = await _context.Rok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rok == null)
            {
                return NotFound();
            }

            return View(rok);
        }

        // POST: Roks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rok = await _context.Rok.FindAsync(id);
            _context.Rok.Remove(rok);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RokExists(int id)
        {
            return _context.Rok.Any(e => e.Id == id);
        }
    }
}
