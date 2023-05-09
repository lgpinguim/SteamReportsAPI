using Microsoft.EntityFrameworkCore;
using SteamReports.Domain.Models;
using SteamReports.Infra.Data.Mappings;

namespace SteamReports.Infra.Data.Context
{
    public sealed class SteamReportsContext : DbContext
    {
        public SteamReportsContext(DbContextOptions<SteamReportsContext> options) : base(options) { }

        public DbSet<SteamApp> Games { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<SteamPlayerCount> SteamPlayerCounts { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SteamAppMap());
            modelBuilder.ApplyConfiguration(new ReviewMap());
            modelBuilder.ApplyConfiguration(new SteamPlayerCountMap());
            modelBuilder.ApplyConfiguration(new IssueMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
