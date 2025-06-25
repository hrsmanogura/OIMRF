using Microsoft.EntityFrameworkCore;
using OIMRF.Models;

namespace OIMRF.Properties.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Complaints> Complaints { get; set; }
        public DbSet<EnvironmentalLaws> EnvironmentalLaws { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
