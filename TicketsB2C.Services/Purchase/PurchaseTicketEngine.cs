using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using TicketsB2C.Services.Configuration;
using TicketsB2C.Services.Repository;

namespace TicketsB2C.Services.Purchase
{
    public class PurchaseTicketEngine : IPurchaseTicketEngine
    {
        private readonly ITicketRepository _tickets;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ExternalServicesConfiguration externalServices;

        private class DiscountResultDto
        {
            public double discountedTotal { get; set; }
        }

        public PurchaseTicketEngine(ITicketRepository ticket, IHttpClientFactory httpClientFactory, IOptions<ExternalServicesConfiguration> options)
        {
            _tickets = ticket;
            _httpClientFactory = httpClientFactory;
            externalServices = options.Value;
        }

        public async Task<BuyTicketResult> PurchaseTicketAsync(int ticketId, int quantity)
        {
            var ticket = await _tickets.GetByIdAsync(ticketId);
            if (ticket == null)
                return new BuyTicketResult(false, "Ticket not found", null, null);

            var request = new
            {
                Quantity = quantity,
                Price = ticket.Price,
                Total = ticket.Price * quantity,
                Type = ticket.Type.ToString()
            };

            var client = _httpClientFactory.CreateClient();
            
            var discountApiUrl = $"{externalServices.DiscountServerBaseUrl}/api/Discount/calculate";
            var response = await client.PostAsJsonAsync(discountApiUrl, request);

            if (!response.IsSuccessStatusCode)
                return new BuyTicketResult(false, "Discount service error", null, ticket);

            var resultObj = await response.Content.ReadFromJsonAsync<DiscountResultDto>();
            var discountedTotal = resultObj?.discountedTotal;

            return new BuyTicketResult(true, "Purchase cost computed", discountedTotal, ticket);
        }
    }
}
