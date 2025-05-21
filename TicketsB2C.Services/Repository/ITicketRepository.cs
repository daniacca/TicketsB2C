using DataAccess.Models;

namespace TicketsB2C.Services.Repository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsByCarrierAsync(int carrierId);
        Task<IEnumerable<Ticket>> SearchTicketsAsync(string departureCity, string destinationCity);
    }
}
