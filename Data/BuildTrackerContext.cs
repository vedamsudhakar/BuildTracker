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
    }
}
