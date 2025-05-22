namespace TicketsB2C.DTO;

public class BuyTicketSummary
{
    public int TicketId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalAmount { get; set; }
}
