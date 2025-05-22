using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketsB2C.Services.Repository
{
    public class TicketRepository : TicketDBRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketsDbContext context) : base(context) { }

        public override async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Carrier)
                .Include(t => t.Departure)
                .Include(t => t.Destination)
                .ToListAsync();
        }

        public override async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Carrier)
                .Include(t => t.Departure)
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCarrierAsync(int carrierId)
        {
            return await _dbSet
                .Include(t => t.Carrier)
                .Include(t => t.Departure)
                .Include(t => t.Destination)
                .Where(t => t.Carrier.Id == carrierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> SearchTicketsAsync(string departureCity, string destinationCity)
        {
            return await _dbSet
                .Include(t => t.Carrier)
                .Include(t => t.Departure)
                .Include(t => t.Destination)
                .Where(t => t.Departure.Name == departureCity && t.Destination.Name == destinationCity)
                .ToListAsync();
        }
    }
}
