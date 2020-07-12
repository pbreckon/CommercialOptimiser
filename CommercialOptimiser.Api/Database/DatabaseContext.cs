using System.Linq;
using CommercialOptimiser.Api.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace CommercialOptimiser.Api.Database
{
    public class DatabaseContext : DbContext
    {
        #region Constructors

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> context) : base(context)
        {
        }

        #endregion

        #region Public Properties

        public DbSet<BreakDemographicTable> BreakDemographics { get; set; }

        public DbSet<BreakTable> Breaks { get; set; }

        public DbSet<CommercialTable> Commercials { get; set; }

        public DbSet<DemographicTable> Demographics { get; set; }

        public DbSet<UserReportBreakCommercialTable> UserReportBreakCommercials { get; set; }

        public DbSet<UserReportBreakTable> UserReportBreaks { get; set; }

        public DbSet<UserTable> Users { get; set; }

        #endregion

        #region Protected Methods

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

            modelBuilder.Entity<UserReportBreakTable>()
                .HasOne(urb => urb.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserReportBreakCommercialTable>()
                .HasOne(urbc => urbc.UserReportBreak)
                .WithMany(urb => urb.UserReportBreakCommercials)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }

    public static class DatabaseContextExtensions
    {
        public static void DetachLocal<T>(this DbContext context, T t)
            where T : class, IBaseTable
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(t.Id));
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }
    }
}