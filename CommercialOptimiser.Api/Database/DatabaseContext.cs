using CommercialOptimiser.Api.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> context) : base(context)
        {
        }

        #region Public Properties

        public DbSet<BreakTable> Breaks { get; set; }

        public DbSet<CommercialTable> Commercials { get; set; }

        public DbSet<DemographicTable> Demographics { get; set; }

        public DbSet<BreakDemographicTable> BreakDemographics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BreakDemographicTable>()
                .HasOne(bd => bd.Break)
                .WithMany(b => b.BreakDemographics)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}