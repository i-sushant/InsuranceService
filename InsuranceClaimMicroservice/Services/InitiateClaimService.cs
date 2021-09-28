using InsuranceClaimMicroservice.Models;
using InsuranceClaimMicroservice.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace InsuranceClaimMicroservice.Services
{
    public class InitiateClaimService : IInitiateClaimService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IConfiguration _configuration;

        public InitiateClaimService(IAuditRepository auditRepository, IConfiguration configuration)
        {
            _auditRepository = auditRepository;
            _configuration = configuration;
        }
        public async Task<long> InitiateClaim(InitiateClaim initiateClaim, string token)
        {
            var insuranceAmount = _auditRepository.GetInsurerByInsurerName(initiateClaim.InsurerName).InsuranceAmountLimit;
            var treatmentPlan = new IPTreatmentPackage();
            using(var httpClient = new HttpClient())
            { 
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var endpoint = $"{_configuration["IPTreatmentOfferingBaseUri"]}/IPTreatmentPackages/IPTreatmentPackageByName/?ailment={(int)initiateClaim.Ailment}&treatmentPackageName={initiateClaim.TreatmentPackageName}";
                var response = await httpClient.GetAsync(endpoint);
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    treatmentPlan = JsonConvert.DeserializeObject<IPTreatmentPackage>(result);
                }
            }
            var total = treatmentPlan.PackageDetail.Cost - insuranceAmount < 0 ? 0 : treatmentPlan.PackageDetail.Cost - insuranceAmount;
            initiateClaim.BalanceAmount = total;
            _auditRepository.AddClaim(initiateClaim);
            return total;
        }         
    }
}
