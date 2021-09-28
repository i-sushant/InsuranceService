using InsuranceClaimMicroservice.Models;
using System.Collections.Generic;

namespace InsuranceClaimMicroservice.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private List<InsurerDetail> _insurers;
        private List<InitiateClaim> _insuranceClaims;
        public AuditRepository()
        {
            _insurers = new List<InsurerDetail>
           {
               new InsurerDetail { InsurerName = "Bajaj Insurance", InsurerPackageName = "L1", InsuranceAmountLimit = 200000, DisbursementDuration = 2},
               new InsurerDetail { InsurerName = "Tata Insurance", InsurerPackageName = "L2", InsuranceAmountLimit = 300000, DisbursementDuration = 1},
               new InsurerDetail { InsurerName = "Reliance Insurance", InsurerPackageName = "L3", InsuranceAmountLimit = 400000, DisbursementDuration = 1},
               new InsurerDetail { InsurerName = "LIC", InsurerPackageName = "L4", InsuranceAmountLimit = 250000, DisbursementDuration = 2},
               new InsurerDetail { InsurerName = "ICICI Insurance", InsurerPackageName = "L5", InsuranceAmountLimit = 280000, DisbursementDuration = 2}
           };
            _insuranceClaims = new List<InitiateClaim>();
        }
        public IEnumerable<InsurerDetail> GetAllInsurers()
        {
            return _insurers;
        }

        public InsurerDetail GetInsurerByPackageName(string packageName)
        {
            return _insurers.Find(insurer => insurer.InsurerPackageName.ToLower() == packageName.ToLower());
        }

        public void AddClaim(InitiateClaim claim)
        {
            claim.Id = _insuranceClaims.Count + 1;
            _insuranceClaims.Add(claim);
        }

        public InsurerDetail GetInsurerByInsurerName(string insurerName)
        {
            return _insurers.Find(insurer => insurer.InsurerName.ToLower() == insurerName.ToLower());
        }

        public InitiateClaim GetClaim(int patientId)
        {
            return _insuranceClaims.Find(claim => claim.PatientId == patientId);
        }
    }
}
