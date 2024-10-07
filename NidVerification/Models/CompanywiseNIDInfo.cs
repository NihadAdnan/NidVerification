using System.ComponentModel.DataAnnotations;

namespace NidVerification.Models
{
    public class CompanywiseNIDInfo
    {
        [Key]
        public int CP_ID { get; set; }
        public string Nid10Digit { get; set; }
        public string Nid13Digit { get; set; }
        public string Nid17Digit { get; set; }
        public bool IsActive { get; set; }
    }
}
