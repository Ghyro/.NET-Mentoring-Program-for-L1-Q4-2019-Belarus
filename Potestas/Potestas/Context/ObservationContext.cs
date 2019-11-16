using Microsoft.EntityFrameworkCore;
using Potestas.Observations.Wrappers;
using System.Configuration;

namespace Potestas.Context
{
    public class ObservationContext : DbContext
    {
        public DbSet<FlashObservationWrapper> FlashObservationWrapper { get; set; }
        public DbSet<CoordinatesWrapper> CoordinatesWrapper { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ObservationsWrapper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlashObservationWrapper>().HasKey(x => x.Id);
            modelBuilder.Entity<CoordinatesWrapper>().HasKey(x => x.Id);

            modelBuilder.Entity<FlashObservationWrapper>()
            .HasOne(x => x.ObservationPoint)
            .WithMany(t => t.FlashObservations)
            .HasForeignKey(p => p.CoordinatesId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
