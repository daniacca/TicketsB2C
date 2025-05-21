namespace DataAccess.Models
{
    public enum TicketType
    {
        Bus,
        Train,
    }

    public class Ticket
    {
        public int Id { get; set; }
        public virtual Company Carrier { get; set; } = new Company();
        public virtual City Departure { get; set; } = new City();
        public virtual City Destination { get; set; } = new City();
        public TicketType Type { get; set; }
        public double Price { get; set; }
    }
}
