using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests(string firstAttribute = null, string innerAtt = null, string search = null)
        {
            var nonProcessed = new List<Polaganja>();

            if (firstAttribute != null && innerAtt != null && search != null)
            {
                nonProcessed = await GetFilteredPolaganja(firstAttribute, innerAtt, search);
                nonProcessed = nonProcessed.Where(p => p.StatusId < (int)SD.StatusPrijave.IspitPolozen).ToList();
            }
            else
            {
                var lazyNonProcessed = _context.Polaganja
                                        .Where(p => p.StatusId < (int)SD.StatusPrijave.IspitPolozen);

                nonProcessed = await IncludeAll(lazyNonProcessed);
            }

            return await FillTakeNumber(nonProcessed);
        }

        public async Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests(int kandidatID)
        {

            var lazyNonProcessed = _context.Polaganja.Where(p => p.KandidatId == kandidatID
                                                                 &&  p.StatusId < (int)SD.StatusPrijave.IspitPolozen);

            IEnumerable<Polaganja> eagerNonProcessed = await IncludeAll(lazyNonProcessed);
                                                                                      
            return await FillTakeNumber(eagerNonProcessed, kandidatID);

        }

        public async Task<IEnumerable<Polaganja>> GetCompletedExams(int kandidatID)
        {
            return await _context.Polaganja
                .Where(p => (p.KandidatId == kandidatID) &&
                            (p.StatusId == (int)SD.StatusPrijave.IspitPolozen || p.StatusId == (int)SD.StatusPrijave.OslobodjenPolaganja))
                .Include(i => i.Ispit).Include(r => r.Rok).OrderBy(i => i.IspitId).ToListAsync();
        }

        public async Task<IEnumerable<Polaganja>> GetProcessedRequests(string firstAttribute = null, string innerAtt = null, string search = null)
        {

            if (firstAttribute!=null && innerAtt!=null && search!=null )
            {
                return (await GetFilteredPolaganja(firstAttribute, innerAtt, search))
                    .Where(p => p.StatusId >= (int)SD.StatusPrijave.IspitPolozen);
            }
            else
            {
                var lazyProcessed = _context.Polaganja
                    .Where(p => p.StatusId >= (int)SD.StatusPrijave.IspitPolozen);

                return await IncludeAll(lazyProcessed);
            }
              
        }

        public async Task<IEnumerable<Polaganja>> GetProcessedRequests(int kandidatID)
        {
            var lazyProcessed = _context.Polaganja
                                    .Where(p => p.KandidatId == kandidatID
                                                && p.StatusId >= (int)SD.StatusPrijave.IspitPolozen);

            return await IncludeAll(lazyProcessed);
        }

        public async Task<int> GetExamTakeNumber(int ispitID, int kandidatID)
        {
            var takeList = await _context.Polaganja.Where(p => p.IspitId == ispitID && p.KandidatId == kandidatID && (p.StatusId>2 && p.StatusId<7)).ToListAsync();
            return takeList.Count();
        }

        public async Task<int> GetExamTakeYTD(int kandidatID)
        {
            var takeList = await _context.Polaganja.Where(p => (p.KandidatId == kandidatID) && (p.RequestDate.Year==DateTime.Now.Year) && p.StatusId > 2 && p.StatusId < 6 ).ToListAsync();
            return takeList.Count();
        }

        private async Task<IEnumerable<NonProcessedExams>> FillTakeNumber(IEnumerable<Polaganja> polaganjas, int kandidatID = 0)
        {
            List<NonProcessedExams> npolaganjas = new List<NonProcessedExams>();

            foreach (var item in polaganjas)
            {
                int ispitID = item.IspitId;

                int id = kandidatID;

                if (kandidatID==0)
                {
                    id = item.KandidatId;
                }

                NonProcessedExams npolaganje = new NonProcessedExams(item)
                {
                    TakeNumber = await GetExamTakeNumber(ispitID, id)
                };

                npolaganjas.Add(npolaganje);
            }

            return npolaganjas;
        }

        private async Task<List<Polaganja>> GetFilteredPolaganja(string filterType, string filterProperty, string filterValue)
        {
            var param = Expression.Parameter(typeof(Polaganja), "p");

            var complexProperty = Expression.Property(param, filterType);

            var innerProperty = Expression.Property(complexProperty, filterProperty);

            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[] { });


            var toLowerProperty = Expression.Call(innerProperty, toLowerMethod);

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });


            var constant = Expression.Constant(filterValue, typeof(string));

            var toLowerValue = Expression.Call(constant, toLowerMethod);


            var body = Expression.Call(toLowerProperty, containsMethod, toLowerValue);

            var exp = Expression.Lambda<Func<Polaganja, bool>>(body, param);


            var filtered = _context.Polaganja
                                    .Where(exp);

            return await IncludeAll(filtered);
        }

        private async Task<List<Polaganja>> IncludeAll(IQueryable<Polaganja> polaganja)
        {
            return await polaganja
                    .Include(p => p.Kandidat)
                    .Include(p => p.Ispit)
                    .Include(p => p.Status)
                    .Include(p => p.Rok)
                    .ToListAsync();
        }
    }
}
