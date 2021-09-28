using InsuranceClaimMicroservice.Models;
using System.Threading.Tasks;

namespace InsuranceClaimMicroservice.Services
{
    public interface IInitiateClaimService
    {
        Task<long> InitiateClaim(InitiateClaim initiateClaim, string token);
    }
}
