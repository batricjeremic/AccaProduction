using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccaProduction.Models;
using AccaProduction.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static AccaProduction.ViewModels.PolaganjasView;

namespace AccaProduction.Repository
{
    public class PolaganjaRepository : IPolaganjaRepository
    {
        private readonly AccaCandidatesContext _context;

        public PolaganjaRepository(AccaCandidatesContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests()
        {
            IEnumerable<Polaganja> polaganjas = await _context.Polaganja.Where(p => p.StatusId < (int)SD.StatusPrijave.IspitPolozen)
                                            .Include(p => p.Kandidat)
                                            .Include(p => p.Ispit)
                                            .Include(p => p.Status)
                                            .Include(p => p.Rok)
                                            .ToListAsync();

            List<NonProcessedExams> npolaganjas = new List<NonProcessedExams>();

            foreach (var item in polaganjas)
            {
                int ispitID = item.IspitId;
                int kandidatID = item.KandidatId;

                NonProcessedExams npolaganje = new NonProcessedExams(item);
                npolaganje.TakeNumber = await GetExamTakeNumber(ispitID, kandidatID);

                npolaganjas.Add(npolaganje);
            }

            return npolaganjas;
        }

        public async Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests(int kandidatID)
        {
            IEnumerable<Polaganja> polaganjas = await _context.Polaganja.Include(p => p.Kandidat)
                                            .Where(k => k.KandidatId == kandidatID)
                                            .Include(p => p.Ispit)
                                            .Include(p => p.Status)
                                            .Include(p => p.Rok)
                                            .Where(p => p.StatusId < (int)SD.StatusPrijave.IspitPolozen)
                                            .ToListAsync();

            List<NonProcessedExams> npolaganjas = new List<NonProcessedExams>();

            foreach (var item in polaganjas)
            {
                int ispitID = item.IspitId;

                NonProcessedExams npolaganje = new NonProcessedExams(item);
                npolaganje.TakeNumber = await GetExamTakeNumber(ispitID, kandidatID);

                npolaganjas.Add(npolaganje);
            }

            return npolaganjas;

        }

        public async Task<IEnumerable<Polaganja>> GetCompletedExams(int kandidatID)
        {
            return await _context.Polaganja
                .Where(p => (p.KandidatId == kandidatID) &&
                            (p.StatusId == (int)SD.StatusPrijave.IspitPolozen || p.StatusId == (int)SD.StatusPrijave.OslobodjenPolaganja))
                .Include(i => i.Ispit).Include(r => r.Rok).OrderBy(i => i.IspitId).ToListAsync();
        }

        public async Task<IEnumerable<Polaganja>> GetProcessedRequests()
        {
            return await _context.Polaganja
                .Include(p => p.Kandidat)
                .Include(p => p.Ispit)
                .Include(p => p.Status)
                .Include(p => p.Rok)
                .Where(p => p.StatusId >= (int)SD.StatusPrijave.IspitPolozen).ToListAsync();
        }

        public async Task<IEnumerable<Polaganja>> GetProcessedRequests(int kandidatID)
        {
            return await _context.Polaganja
                            .Include(p => p.Kandidat)
                            .Where(k=>k.KandidatId==kandidatID)
                            .Include(p => p.Ispit)
                            .Include(p => p.Status)
                            .Include(p => p.Rok)
                            .Where(p => p.StatusId >= (int)SD.StatusPrijave.IspitPolozen).ToListAsync();
        }

        public async Task<int> GetExamTakeNumber(int ispitID, int kandidatID)
        {
            var takeList = await _context.Polaganja.Where(p => p.IspitId == ispitID && p.KandidatId == kandidatID).ToListAsync();
            return takeList.Count();
        }

        public async Task<int> GetExamTakeYTD(int kandidatID)
        {
            var takeList = await _context.Polaganja.Where(p => (p.KandidatId == kandidatID) && (p.RequestDate.Year==DateTime.Now.Year)).ToListAsync();
            return takeList.Count();
        }
    }
}
