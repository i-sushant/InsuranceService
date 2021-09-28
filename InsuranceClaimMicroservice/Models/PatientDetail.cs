using System;

namespace InsuranceClaimMicroservice.Models
{
    public class PatientDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public AilmentCategory Ailment { get; set; }
        public string TreatmentPackageName { get; set; }
        public DateTime TreatmentCommencementDate { get; set; }
    }
}
