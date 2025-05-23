namespace TicketsB2C.DTO;

public class BuyTicketResponse
{
    public int TicketId { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalAmount { get; set; }
    public string Departure { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Carrier { get; set; } = string.Empty;
}
