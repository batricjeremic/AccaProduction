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


        public async Task<List<Kandidat>> GetFilteredKandidats(string filterProperty, string filterValue)
        {
            var param = Expression.Parameter(typeof(Kandidat), "k");

            var property = Expression.Property(param, filterProperty);

            var toLowerMethod = typeof(string).GetMethod("ToLower", new Type[] { });


            var toLowerProperty = Expression.Call(property, toLowerMethod);

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });


            var constant = Expression.Constant(filterValue, typeof(string));

            var toLowerValue = Expression.Call(constant, toLowerMethod);


            var body = Expression.Call(toLowerProperty, containsMethod, toLowerValue);

            var exp = Expression.Lambda<Func<Kandidat, bool>>(body, param);

            
            return await _context.Kandidat.Where(exp).ToListAsync();
        }

        public async Task<int> GetIdByEmail(string email)
        {
            return await _context.Kandidat.Where(k => k.Email == email).Select(i => i.IdAccaNumber).FirstOrDefaultAsync();
        }

        public async Task<Kandidat> GetKandidat(int id)
        {
            return await _context.Kandidat.FindAsync(id);
        }

    }
}
