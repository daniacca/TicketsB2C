using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace DataAccess
{
    public class TicketsDbContext : DbContext
    {
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ticket -> Company (Carrier)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Carrier)
                .WithMany()
                .HasForeignKey("CarrierId")
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> City (Departure)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Departure)
                .WithMany()
                .HasForeignKey("DepartureId")
                .OnDelete(DeleteBehavior.Restrict);

            // Ticket -> City (Destination)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Destination)
                .WithMany()
                .HasForeignKey("DestinationId")
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
