using CivittaTest.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CivittaTest.DAL.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Country> Countries { get; set; } = null!;

        public DbSet<Region> Regions { get; set; } = null!;

        public DbSet<Holiday> Holidays { get; set; } = null!;

        public DbSet<DayStatus> DayStatuses { get; set; }

        public DbSet<MaximumFreeDayInRow> MaximumFreeDayInRows { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasIndex(c => c.CountryCode);

            modelBuilder.Entity<Region>().HasIndex(c => c.Code);
        }
    }
}
