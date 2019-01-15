using AccaProduction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccaProduction.ViewModels.PolaganjasView;

namespace AccaProduction.Repository
{
    public interface IPolaganjaRepository
    {

        Task<IEnumerable<Polaganja>> GetCompletedExams(int kandidatID);

        Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests();
        Task<IEnumerable<NonProcessedExams>> GetNonProcessedRequests(int kandidatID);


        Task<IEnumerable<Polaganja>> GetProcessedRequests();
        Task<IEnumerable<Polaganja>> GetProcessedRequests(int kandidatID);

        Task<int> GetExamTakeNumber(int ispitID, int kandidatID);
        Task<int> GetExamTakeYTD(int kandidatID);

    }
}