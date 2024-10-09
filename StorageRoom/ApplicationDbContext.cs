using Microsoft.EntityFrameworkCore;
using StorageRoom.Models.Entity;

namespace StorageRoom
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Baggage> Baggages { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Passenger>()
           .HasOne(p => p.Flight)
           .WithMany(f => f.Passengers)
           .HasForeignKey(p => p.FlightId);

            modelBuilder.Entity<Baggage>()
                .HasOne(b => b.Passenger)
                .WithMany(p => p.Baggages)
                .HasForeignKey(b => b.PassengerId);
        }
    }


}
