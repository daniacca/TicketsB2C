namespace TicketsB2C.Services.Purchase
{
    public interface IPurchaseTicketEngine
    {
        Task<BuyTicketResult> PurchaseTicketAsync(int ticketId, int quantity);
    }
}
