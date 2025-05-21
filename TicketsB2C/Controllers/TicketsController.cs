using Microsoft.AspNetCore.Mvc;
using TicketsB2C.DTO;

namespace TicketsB2C.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : Controller
    {

        public TicketsController()
        {
            
        }

        // GET: api/Tickets
        [HttpGet]
        public IActionResult GetTickets()
        {
            return Ok(new List<TicketApiOutput> { });
        }

        // GET: api/Tickets/carrier/{carrierId}
        [HttpGet("carrier/{carrierId}")]
        public IActionResult GetTicketsByCarrier(int carrierId)
        {
            return Ok(new List<TicketApiOutput> { });
        }

        // GET: api/Tickets/search?departureCity={departureCity}&destinationCity={destinationCity}
        [HttpGet("search")]
        public IActionResult SearchTickets([FromQuery] string departureCity, [FromQuery] string destinationCity)
        {
            return Ok(new List<TicketApiOutput> { });
        }
    }
}
