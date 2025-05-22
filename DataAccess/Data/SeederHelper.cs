using DataAccess.Models;

namespace TicketsModel.Data
{
    public static class SeederHelper
    {
        public static List<City> Cities =>
        [
            new City { Name = "Roma" },
            new City { Name = "Milano" },
            new City { Name = "Napoli" },
            new City { Name = "Torino" },
            new City { Name = "Palermo" },
            new City { Name = "Genova" },
            new City { Name = "Bologna" },
            new City { Name = "Firenze" },
            new City { Name = "Bari" },
            new City { Name = "Catania" },
            new City { Name = "Venezia" },
            new City { Name = "Verona" },
            new City { Name = "Messina" },
            new City { Name = "Padova" },
            new City { Name = "Trieste" },
            new City { Name = "Taranto" },
            new City { Name = "Brescia" },
            new City { Name = "Prato" },
            new City { Name = "Parma" },
            new City { Name = "Modena" }
        ];

        public static List<Company> Companies =>
        [
            new Company { Name = "Blue Travels", Description = "Viaggi in pullman e treno in tutta Italia", PhoneNumber = "0612345678", Email = "info@bluetravels.it" },
            new Company { Name = "Red Transports", Description = "Trasporti rapidi e affidabili", PhoneNumber = "0623456789", Email = "contatti@redtransports.it" },
            new Company { Name = "Green Mobility", Description = "Mobilità sostenibile per tutti", PhoneNumber = "0634567890", Email = "support@greenmobility.it" },
            new Company { Name = "ItalGo", Description = "Viaggi comodi e veloci", PhoneNumber = "0645678901", Email = "info@italgo.it" },
            new Company { Name = "ViaggiaSereno", Description = "Spostamenti senza pensieri", PhoneNumber = "0656789012", Email = "info@viaggiasereno.it" }
        ];

        public static List<Ticket> Tickets =>
        [
            new Ticket { Carrier = Companies[0], Departure = Cities[0], Destination = Cities[1], Type = TicketType.Bus, Price = 15.50 },
            new Ticket { Carrier = Companies[1], Departure = Cities[1], Destination = Cities[2], Type = TicketType.Train, Price = 22.00 },
            new Ticket { Carrier = Companies[2], Departure = Cities[2], Destination = Cities[3], Type = TicketType.Bus, Price = 18.75 },
            new Ticket { Carrier = Companies[3], Departure = Cities[3], Destination = Cities[4], Type = TicketType.Train, Price = 25.00 },
            new Ticket { Carrier = Companies[4], Departure = Cities[4], Destination = Cities[0], Type = TicketType.Bus, Price = 20.00 },
            new Ticket { Carrier = Companies[0], Departure = Cities[5], Destination = Cities[6], Type = TicketType.Train, Price = 30.00 },
            new Ticket { Carrier = Companies[1], Departure = Cities[6], Destination = Cities[7], Type = TicketType.Bus, Price = 12.00 },
            new Ticket { Carrier = Companies[2], Departure = Cities[7], Destination = Cities[8], Type = TicketType.Train, Price = 28.00 },
            new Ticket { Carrier = Companies[3], Departure = Cities[8], Destination = Cities[9], Type = TicketType.Bus, Price = 16.50 },
            new Ticket { Carrier = Companies[4], Departure = Cities[9], Destination = Cities[10], Type = TicketType.Train, Price = 24.00 }
        ];
    }
}
