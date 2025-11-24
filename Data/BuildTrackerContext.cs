using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BuildTracker.Models;

namespace BuildTracker.Data
{
    public class BuildTrackerContext : IdentityDbContext<IdentityUser>
    {
        public BuildTrackerContext(DbContextOptions<BuildTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<BuildInfo> Builds { get; set; }
        public DbSet<FtpServer> FtpServers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<BugAttachment> BugAttachments { get; set; }
        public DbSet<BugComment> BugComments { get; set; }
        public DbSet<BugHistory> BugHistory { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
    }
}
