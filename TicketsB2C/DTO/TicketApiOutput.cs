namespace TicketsB2C.DTO
{
    public class TicketApiOutput
    {
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Carrier { get; set; } = string.Empty;
    }
}
