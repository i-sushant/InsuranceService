using InsuranceClaimMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceClaimMicroservice.Repository
{
    public interface IAuditRepository
    {
        IEnumerable<InsurerDetail> GetAllInsurers();
        InsurerDetail GetInsurerByPackageName(string packageName);
        InsurerDetail GetInsurerByInsurerName(string insurerName);
        void AddClaim(InitiateClaim claim);

        InitiateClaim GetClaim(int patientId);

    }
}
