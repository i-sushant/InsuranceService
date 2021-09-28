namespace InsuranceClaimMicroservice.Models
{
    public class InitiateClaim
    {
        public int Id { get; set; } = 0;
        public string PatientName { get; set; }
        public int PatientId { get; set; }
        public AilmentCategory Ailment { get; set; }
        public string TreatmentPackageName { get; set; }
        public string InsurerName { get; set; }
        public long BalanceAmount { get; set; } = 0;
    }
}
