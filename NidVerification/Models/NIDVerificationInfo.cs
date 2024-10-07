using System.ComponentModel.DataAnnotations;

namespace NidVerification.Models
{
    public class NIDVerificationInfo
    {
        [Key]
        public int Id { get; set; }
        public int CP_ID { get; set; }
        public string TransID { get; set; }
        public DateTime TransDate { get; set; }
        public string RequestData { get; set; }
        public string RequestKey { get; set; }
        public string ResponseData { get; set; }
        public int Status { get; set; }
        public string RequestIP { get; set; }
        public string RequestFrom { get; set; }
        public string ReferrerPage { get; set; }
    }
}
