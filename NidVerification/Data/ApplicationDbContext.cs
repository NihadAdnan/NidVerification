using Microsoft.EntityFrameworkCore;
using NidVerification.Models;

namespace NidVerification.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<NIDVerificationInfo> NIDVerificationInfo { get; set; }
        public DbSet<CompanywiseNIDInfo> CompanywiseNIDInfo { get; set; }
        public DbSet<CompanyVerificationStatus> CompanyVerificationStatus { get; set; }
        public DbSet<CompanyVerificationLegalInfo> CompanyVerificationLegalInfo { get; set; }
    }
}
