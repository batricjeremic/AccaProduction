using AccaProduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccaProduction.Repository
{
    public interface IIspitRepository
    {
        Task<Ispit> GetIspit(int ispitID);

        Task<IEnumerable<Ispit>> GetNepolozeniIspiti(int kandidatID);
    }
}
