using Microsoft.EntityFrameworkCore;
using Potestas.Observations;
using System.Configuration;

namespace Potestas.Context
{
    public class ObservationContext : DbContext
    {
        public DbSet<FlashObservation> FlashObservations { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Observations;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlashObservation>().HasKey(x => x.Id);
            modelBuilder.Entity<Coordinates>().HasKey(x => x.Id);

            modelBuilder.Entity<FlashObservation>()
            .HasOne(x => x.ObservationPoint)
            .WithMany(t => t.FlashObservations)
            .HasForeignKey(p => p.CoordinatesId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
