namespace InsuranceClaimMicroservice.Models
{
    public class InsurerDetail
    {
        public string InsurerName { get; set; }
        public string InsurerPackageName { get; set; }
        public long InsuranceAmountLimit { get; set; }
        public long DisbursementDuration { get; set; }
    }
}
