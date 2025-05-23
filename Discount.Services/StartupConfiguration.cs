using Discount.Services.Configurations;
using Discount.Services.Interfaces;
using Discount.Services.Discount;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.Services
{
    public static class StartupConfiguration
    {
        public static IServiceCollection AddDiscountServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind configuration from appsettings.json
            services.Configure<DiscountRulesConfig>(option => configuration.GetSection("DiscountRules").Bind(option));
            services.Configure<DiscountEngineConfig>(option => configuration.GetSection("DiscountEngine").Bind(option));

            // Register the discount rules, put there all the interfaces that implement IDiscountRule
            services.AddScoped<IDiscountRule, QuantityDiscountRule>();
            services.AddScoped<IDiscountRule, CarrierTypeDiscountRule>();

            // Register the discount engine
            services.AddScoped<IDiscountEngine, DiscountEngine>();
            return services;
        }
    }
}
