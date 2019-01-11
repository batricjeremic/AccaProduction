using AccaProduction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccaProduction.Repository
{
    public interface IKandidatRepository
    {
        Task<List<Kandidat>> GetFilteredKandidats(string parameter, string value);
    }
}