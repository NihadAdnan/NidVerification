using System.ComponentModel.DataAnnotations;

namespace NidVerification.Models
{
    public class CompanyVerificationLegalInfo
    {
        [Key]
        public int CP_ID { get; set; }
        public string LegalNo { get; set; }
        public string LegalType { get; set; }
        public int PostedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}
