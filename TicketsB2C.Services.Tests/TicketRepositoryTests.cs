using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using TicketsB2C.Services.Repository;
using Xunit;

namespace TicketsB2C.Services.Tests
{
    public class TicketRepositoryTests
    {
        private TicketsDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TicketsDbContext>()
                .UseInMemoryDatabase(databaseName: "TicketsTestDb_" + System.Guid.NewGuid())
                .Options;
            return new TicketsDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTickets_WithIncludes()
        {
            using var context = CreateDbContext();
            var company = new Company { Id = 1, Name = "Compagnia" };
            var cityA = new City { Id = 1, Name = "A" };
            var cityB = new City { Id = 2, Name = "B" };
            var ticket = new Ticket
            {
                Id = 1,
                Carrier = company,
                Departure = cityA,
                Destination = cityB,
                Type = TicketType.Bus,
                Price = 10
            };

            context.Companies.Add(company);
            context.Cities.AddRange(cityA, cityB);
            context.Tickets.Add(ticket);

            await context.SaveChangesAsync();

            var repo = new TicketRepository(context);
            var result = await repo.GetAllAsync();

            var loaded = Assert.Single(result);
            Assert.Equal("Compagnia", loaded.Carrier.Name);
            Assert.Equal("A", loaded.Departure.Name);
            Assert.Equal("B", loaded.Destination.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTicket_WhenExists()
        {
            using var context = CreateDbContext();
            var ticket = new Ticket 
            { 
                Id = 2, 
                Carrier = new Company(), 
                Departure = new City(), 
                Destination = new City(), 
                Type = TicketType.Train, 
                Price = 20 
            };
            
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            var repo = new TicketRepository(context);
            var result = await repo.GetByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public async Task GetTicketsByCarrierAsync_ReturnsTicketsForCarrier()
        {
            using var context = CreateDbContext();
            var company = new Company { Id = 3, Name = "C" };
            var ticket = new Ticket 
            { 
                Id = 3, 
                Carrier = company, 
                Departure = new City(), 
                Destination = new City(), 
                Type = TicketType.Bus, 
                Price = 30 
            };
            
            context.Companies.Add(company);
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            var repo = new TicketRepository(context);

            var result = await repo.GetTicketsByCarrierAsync(3);

            var loaded = Assert.Single(result);
            Assert.Equal(3, loaded.Carrier.Id);
        }

        [Fact]
        public async Task SearchTicketsAsync_ReturnsTicketsForCities()
        {
            using var context = CreateDbContext();
            var dep = new City { Id = 4, Name = "Roma" };
            var dest = new City { Id = 5, Name = "Milano" };
            var ticket = new Ticket 
            { 
                Id = 4, 
                Carrier = new Company(), 
                Departure = dep, 
                Destination = dest, 
                Type = TicketType.Bus, 
                Price = 40 
            };
            
            context.Cities.AddRange(dep, dest);
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();

            var repo = new TicketRepository(context);

            var result = await repo.SearchTicketsAsync("Roma", "Milano");

            var loaded = Assert.Single(result);
            Assert.Equal("Roma", loaded.Departure.Name);
            Assert.Equal("Milano", loaded.Destination.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenNoTickets()
        {
            using var context = CreateDbContext();
            var repo = new TicketRepository(context);

            var result = await repo.GetAllAsync();

            Assert.Empty(result);
        }


        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTicketDoesNotExist()
        {
            using var context = CreateDbContext();
            var repo = new TicketRepository(context);

            var result = await repo.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetTicketsByCarrierAsync_ReturnsEmpty_WhenCarrierDoesNotExist()
        {
            using var context = CreateDbContext();
            var repo = new TicketRepository(context);
            var result = await repo.GetTicketsByCarrierAsync(123);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchTicketsAsync_ReturnsEmpty_WhenCitiesDoNotMatch()
        {
            using var context = CreateDbContext();
            var repo = new TicketRepository(context);
            var result = await repo.SearchTicketsAsync("NonEsiste", "NeancheQuesta");
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchTicketsAsync_ReturnsEmpty_WhenParametersAreNullOrEmpty()
        {
            using var context = CreateDbContext();
            var repo = new TicketRepository(context);

            var result1 = await repo.SearchTicketsAsync(null, "Dest");
            var result2 = await repo.SearchTicketsAsync("Dep", null);
            var result3 = await repo.SearchTicketsAsync("", "");

            Assert.Empty(result1);
            Assert.Empty(result2);
            Assert.Empty(result3);
        }

        [Fact]
        public async Task GetTicketsByCarrierAsync_ReturnsAllTicketsForCarrier()
        {
            using var context = CreateDbContext();
            var company = new Company { Id = 10, Name = "Multi" };
            var t1 = new Ticket 
            { 
                Carrier = company, 
                Departure = new City(),
                Destination = new City(),
                Type = TicketType.Bus, 
                Price = 1 
            };
            var t2 = new Ticket 
            { 
                Carrier = company, 
                Departure = new City(), 
                Destination = new City(), 
                Type = TicketType.Train, 
                Price = 2 
            };
            
            context.Companies.Add(company);
            context.Tickets.AddRange(t1, t2);
            await context.SaveChangesAsync();

            var repo = new TicketRepository(context);
            var result = await repo.GetTicketsByCarrierAsync(10);

            Assert.Equal(2, result.Count());
        }

    }
}
