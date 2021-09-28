using InsuranceClaimMicroservice.Models;
using InsuranceClaimMicroservice.Repository;
using InsuranceClaimMicroservice.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InsuranceClaimMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditSeverityController : ControllerBase
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IInitiateClaimService _initiateClaimService;
        private readonly IConfiguration _configuration;

        public AuditSeverityController(IAuditRepository auditRepository, IInitiateClaimService initiateClaimService, IConfiguration configuration)
        {
            _auditRepository = auditRepository;
            _initiateClaimService = initiateClaimService;
            _configuration = configuration;

        }
        
        [HttpGet]
        [Route("GetAllInsurerDetail")]
        public IActionResult GetAllInsurerDetail()
        {
            var insurers = _auditRepository.GetAllInsurers();
            if (insurers.Count() == 0) return NotFound();
            else return Ok(insurers);
        }

        [HttpGet]
        [Route("GetInsurerByPackageName")]
        public IActionResult GetInsurerByPackageName(string packageName)
        {
            var insurer = _auditRepository.GetInsurerByPackageName(packageName);
            if (insurer is null) return NotFound();
            else return Ok(insurer);
        }
        [HttpGet]
        [Route("Claim")]
        public IActionResult GetClaim(int patientId)
        {
            var claim = _auditRepository.GetClaim(patientId);
            if (claim is null)
            {
                return NotFound();
            }
            return Ok(claim);
        }
        [HttpPost]
        [Route("InitiateClaim")]
        public async Task<IActionResult> InitiateClaim(InitiateClaim initiateClaim)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (_auditRepository.GetClaim(initiateClaim.PatientId) != null) return BadRequest("Claim already settled");
            if (!(await ValidateClaim(initiateClaim, token))) return BadRequest();
            return Ok(await _initiateClaimService.InitiateClaim(initiateClaim, token));
        }
        private async Task<bool> ValidateClaim(InitiateClaim claim, string token)
        {
            
            if((claim.Ailment != AilmentCategory.Orthopaedics && claim.Ailment != AilmentCategory.Urology))
            {
                return false;
            }
            if (_auditRepository.GetInsurerByInsurerName(claim.InsurerName) is null) return false;
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"{_configuration["IPTreatmentBaseUri"]}/Patient/{claim.PatientId}");
                PatientDetail patient = null;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    patient = JsonConvert.DeserializeObject<PatientDetail>(result);
                    if (patient.Name.ToLower() != claim.PatientName.ToLower()) return false;
                    if (patient.TreatmentPackageName.ToLower() != claim.TreatmentPackageName.ToLower()) return false;
                    if (patient.Ailment != claim.Ailment) return false;
                }
                else
                {
                    return false;
                }
            }
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync($"{_configuration["IPTreatmentOfferingBaseUri"]}/IPTreatmentPackages/Package/?treatmentPackageName={claim.TreatmentPackageName}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK) return false;
            }
            return true;
        }
        
    }
}
