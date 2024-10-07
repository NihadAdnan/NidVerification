using System.ComponentModel.DataAnnotations;

namespace NidVerification.Models
{
    public class CompanyVerificationStatus
    {
        [Key]
        public int CP_ID { get; set; }
        public int vStatus { get; set; }
        public string CollectedDocuments { get; set; }
        public int DocumentCollected { get; set; }
        public int PostedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string FormNo { get; set; }
    }
}
