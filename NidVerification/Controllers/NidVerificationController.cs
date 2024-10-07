using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NidVerification.Data;
using NidVerification.Models;

namespace NidVerification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NIDVerificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NIDVerificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("verify")]
        public IActionResult VerifyNID([FromBody] NIDVerificationRequest request)
        {
            // Decode company ID from base64 and convert to int
            int companyId = int.Parse(DecodeBase64(request.CompanyId));

            // Check if NID already exists
            var existingNid = GetExistingNID(companyId, request.NidNum);
            if (existingNid != null)
            {
                return BadRequest(new { success = false, err = "NID already used for verification" });
            }

            // Check verification status
            var verificationStatus = _context.NIDVerificationInfo
                .Where(v => v.CP_ID == companyId)
                .GroupBy(v => new { v.CP_ID, v.Status })
                .Select(g => new { g.Key.CP_ID, g.Key.Status, Count = g.Count() })
                .OrderByDescending(x => x.Status)
                .FirstOrDefault();

            if (verificationStatus != null && (verificationStatus.Status == 1 || verificationStatus.Count >= 3))
            {
                return BadRequest(new { success = false, err = "Exceeded maximum attempts" });
            }

            // Call external NID verification API
            var verificationResult = CallNIDVerificationAPI(request);

            if (verificationResult.PassKYC && verificationResult.FaceMatched)
            {
                SaveVerificationData(companyId, request.NidNum, verificationResult);
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, err = "NID verification failed" });
        }

        // Helper methods

        private bool IsValidNID(string nidNum)
        {
            return nidNum.Length == 10 || nidNum.Length == 13 || nidNum.Length == 17;
        }

        private string DecodeBase64(string encoded)
        {
            var data = System.Convert.FromBase64String(encoded);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        private CompanywiseNIDInfo GetExistingNID(int companyId, string nidNum)
        {
            return _context.CompanywiseNIDInfo
                .Where(nid => nid.CP_ID == companyId &&
                              (nid.Nid10Digit == nidNum || nid.Nid13Digit == nidNum || nid.Nid17Digit == nidNum))
                .FirstOrDefault();
        }

        private NIDVerificationInfo SaveVerificationData(int companyId, string nidNum, VerificationResult verificationResult)
        {
            var newVerificationInfo = new NIDVerificationInfo
            {
                CP_ID = companyId,
                TransDate = DateTime.Now,
                RequestData = "", // Set request data as needed
                RequestKey = nidNum,
                ResponseData = "", // Set response data as needed
                Status = 1,
                RequestIP = HttpContext.Connection.RemoteIpAddress.ToString(),
                RequestFrom = Request.Headers["Referer"].ToString(),
                ReferrerPage = Request.Path
            };

            // _context.NIDVerificationInfo.Add(newVerificationInfo);
            // _context.SaveChanges(); // Remove this to prevent saving to DB

            return newVerificationInfo; // Return the constructed object
        }


        private VerificationResult CallNIDVerificationAPI(NIDVerificationRequest request)
        {
            // Call to external API
            return new VerificationResult { PassKYC = true, FaceMatched = true };
        }
    }

    // Request and Result classes

    public class NIDVerificationRequest
    {
        public string CompanyId { get; set; }
        public string NidNum { get; set; }
        public string BirthDate { get; set; }
        public string Img { get; set; }
    }

    public class VerificationResult
    {
        public bool PassKYC { get; set; }
        public bool FaceMatched { get; set; }
    }
}
