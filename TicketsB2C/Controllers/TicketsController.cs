using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using TicketsB2C.DTO;
using TicketsB2C.Services.Repository;

namespace TicketsB2C.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _tickets;

        private TicketApiOutput Map(Ticket t) => new TicketApiOutput
        {
            Departure = t.Departure.Name,
            Destination = t.Destination.Name,
            Type = $"{t.Type}",
            Price = t.Price,
            Carrier = t.Carrier.Name
        };

        public TicketsController(ITicketRepository tickets)
        {
            _tickets = tickets;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _tickets.GetAllAsync();
            return Ok(tickets.Select(Map));
        }

        // GET: api/Tickets/carrier/{carrierId}
        [HttpGet("carrier/{carrierId}")]
        public async Task<IActionResult> GetTicketsByCarrier(int carrierId)
        {
            var tickets = await _tickets.GetTicketsByCarrierAsync(carrierId);
            return Ok(tickets.Select(Map));
        }

        // GET: api/Tickets/search?departureCity={departureCity}&destinationCity={destinationCity}
        [HttpGet("search")]
        public async Task<IActionResult> SearchTickets([FromQuery] string departureCity, [FromQuery] string destinationCity)
        {
            var tickets = await _tickets.SearchTicketsAsync(departureCity, destinationCity);
            return Ok(tickets.Select(Map));
        }
    }
}
