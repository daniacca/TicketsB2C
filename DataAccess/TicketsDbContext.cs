using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using TicketsModel.Data;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSeeding((context, _) =>
            {
                // Seed cities
                var citiesSet = context.Set<City>();
                foreach (var city in SeederHelper.Cities)
                {
                    if (!citiesSet.Any(c => c.Name == city.Name))
                    {
                        citiesSet.Add(city);
                    }
                }

                // Seed companies
                var companiesSet = context.Set<Company>();
                foreach (var company in SeederHelper.Companies)
                {
                    if (!companiesSet.Any(c => c.Name == company.Name))
                    {
                        companiesSet.Add(company);
                    }
                }

                context.SaveChanges();

                // Seed tickets
                var ticketsSet = context.Set<Ticket>();
                foreach (var ticket in SeederHelper.Tickets)
                {
                    // Recupera le entità correlate dal database
                    var departure = citiesSet.FirstOrDefault(c => c.Name == ticket.Departure.Name);
                    var destination = citiesSet.FirstOrDefault(c => c.Name == ticket.Destination.Name);
                    var carrier = companiesSet.FirstOrDefault(c => c.Name == ticket.Carrier.Name);

                    // Associa le entità esistenti al ticket
                    ticket.Departure = departure;
                    ticket.Destination = destination;
                    ticket.Carrier = carrier;

                    // Verifica se il ticket esiste già
                    if (!ticketsSet.Any(t =>
                        t.Departure.Name == ticket.Departure.Name &&
                        t.Destination.Name == ticket.Destination.Name &&
                        t.Type == ticket.Type))
                    {
                        ticketsSet.Add(ticket);
                    }
                }

                // Save tickets on DB
                context.SaveChanges();
            })
            .UseAsyncSeeding(async (context, _, token) =>
            {
                token.ThrowIfCancellationRequested();

                // Seed cities
                var citiesSet = context.Set<City>();
                foreach (var city in SeederHelper.Cities)
                {
                    if (!await citiesSet.AnyAsync(c => c.Name == city.Name, token))
                    {
                        await citiesSet.AddAsync(city, token);
                    }
                }

                // Seed companies
                var companiesSet = context.Set<Company>();
                foreach (var company in SeederHelper.Companies)
                {
                    if (!await companiesSet.AnyAsync(c => c.Name == company.Name, token))
                    {
                        await companiesSet.AddAsync(company, token);
                    }
                }

                // Save cities and companies on DB
                await context.SaveChangesAsync(token);

                // Seed tickets
                var ticketsSet = context.Set<Ticket>();
                foreach (var ticket in SeederHelper.Tickets)
                {
                    var departure = await citiesSet.FirstOrDefaultAsync(c => c.Name == ticket.Departure.Name, token);
                    var destination = await citiesSet.FirstOrDefaultAsync(c => c.Name == ticket.Destination.Name, token);
                    var carrier = await companiesSet.FirstOrDefaultAsync(c => c.Name == ticket.Carrier.Name, token);

                    ticket.Departure = departure;
                    ticket.Destination = destination;
                    ticket.Carrier = carrier;

                    if (!await ticketsSet.AnyAsync(t =>
                        t.Departure.Name == ticket.Departure.Name &&
                        t.Destination.Name == ticket.Destination.Name &&
                        t.Type == ticket.Type, token))
                    {
                        await ticketsSet.AddAsync(ticket, token);
                    }
                }

                // Save tickets on DB
                await context.SaveChangesAsync(token);
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
