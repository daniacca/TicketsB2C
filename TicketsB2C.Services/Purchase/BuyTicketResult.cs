using DataAccess.Models;

namespace TicketsB2C.Services.Purchase
{
    public record struct BuyTicketResult(bool Success, string Message, double? PurchasedCost, Ticket? Ticket);
}
