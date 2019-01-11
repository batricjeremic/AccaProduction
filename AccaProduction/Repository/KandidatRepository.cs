using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AccaProduction.Models;
using Microsoft.EntityFrameworkCore;

namespace AccaProduction.Repository
{
    public class KandidatRepository : IKandidatRepository
    {
        private readonly AccaCandidatesContext _context;

        public KandidatRepository(AccaCandidatesContext context)
        {
            this._context = context;
        }


        public async Task<List<Kandidat>> GetFilteredKandidats(string parameter, string value)
        {
            var param = Expression.Parameter(typeof(Kandidat), "k");



            var exp = Expression.Lambda<Func<Kandidat, bool>>(


                Expression.Equal(
                    Expression.Property(param, parameter.ToLower()),
                    Expression.Constant(value.ToLower())
                ),
                param
            );
            return await _context.Kandidat.Where(exp).ToListAsync();
        }

    }
}
