using DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketsB2C.Services.Configuration;
using TicketsB2C.Services.Purchase;
using TicketsB2C.Services.Repository;

namespace TicketsB2C.Services
{
    public static class StartupConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            services.Configure<ExternalServicesConfiguration>(option => configuration.GetSection("ExternalServices").Bind(option));
            services.AddScoped(typeof(IRepository<>), typeof(TicketDBRepository<>));
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPurchaseTicketEngine, PurchaseTicketEngine>();
            return services;
        }
    }
}
