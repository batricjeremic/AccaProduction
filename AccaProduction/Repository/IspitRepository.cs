using AccaProduction.Models;
using AccaProduction.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.Repository
{
    public class IspitRepository : IIspitRepository
    {
        private readonly AccaCandidatesContext _context;

        public IspitRepository(AccaCandidatesContext context)
        {
            this._context = context;
        }

        public async Task<Ispit> GetIspit(int ispitID)
        {
            return await _context.Ispit.FindAsync(ispitID);
        }

        public async Task<IEnumerable<Ispit>> GetNepolozeniIspiti(int kandidatID)
        {
            return await _context.Ispit.Include(p => p.Polaganja)
                .Where(i => i.Polaganja.All(k => !(k.KandidatId == kandidatID)
                                                || (k.StatusId != (int)SD.StatusPrijave.IspitPolozen && k.StatusId != (int)SD.StatusPrijave.OslobodjenPolaganja)))
                .OrderBy(i => i.Id).ToListAsync();
        }
    }
}
