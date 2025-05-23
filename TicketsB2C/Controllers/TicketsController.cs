using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using TicketsB2C.DTO;
using TicketsB2C.Services.Purchase;
using TicketsB2C.Services.Repository;

namespace TicketsB2C.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _tickets;

        private readonly ILogger<TicketsController> _logger;

        private TicketResponse Map(Ticket t) => new TicketResponse
        {
            TicketId = t.Id,
            Departure = t.Departure.Name,
            Destination = t.Destination.Name,
            Type = $"{t.Type}",
            Price = t.Price,
            Carrier = t.Carrier.Name
        };

        public TicketsController(ITicketRepository tickets, ILogger<TicketsController> logger)
        {
            _tickets = tickets;
            _logger = logger;
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

        // POST: api/Tickets/buy
        [HttpPost("buy")]
        public async Task<IActionResult> BuyTicket([FromBody] BuyTicketRequest request, [FromServices] IPurchaseTicketEngine purchaseTicket)
        {
            if (request.Quantity <= 0)
                return BadRequest(new { Message = "La quantità deve essere maggiore di zero." });

            var (Success, Message, PurchasedCost, Ticket) = await purchaseTicket.PurchaseTicketAsync(request.TicketId, request.Quantity);
            
            if (Ticket is null)
                return NotFound(new { Message });

            if(PurchasedCost is null || !Success)
                return BadRequest(new { Message });

            var summary = new BuyTicketResponse
            {
                TicketId = Ticket.Id,
                Quantity = request.Quantity,
                UnitPrice = Ticket.Price,
                TotalAmount = PurchasedCost.Value,
                Departure = Ticket.Departure.Name,
                Destination = Ticket.Destination.Name,
                Type = $"{Ticket.Type}",
                Carrier = Ticket.Carrier.Name
            };

            return Ok(summary);
        }
    }
}
