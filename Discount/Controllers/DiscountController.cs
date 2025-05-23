using Discount.DTO;
using Discount.Services.Discount;
using Discount.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(ILogger<DiscountController> logger)
        {
            _logger = logger;
        }

        // POST: api/Discount/calculate
        [HttpPost("calculate")]
        public ActionResult<double> Calculate([FromBody] CalculateDiscountRequest request, [FromServices] IDiscountEngine discountEngine)
        {
            if (request is null || request.Quantity < 0 || request.Total < 0 || request.Price < 0)
                return BadRequest("Input non valido.");

            var discountedTotal = discountEngine.ApplyAll(new PurchasedTicketData(
                request.Quantity,
                request.Price,
                request.Type,
                request.Total
            ));  

            return Ok(new { discountedTotal });
        }
    }
}
