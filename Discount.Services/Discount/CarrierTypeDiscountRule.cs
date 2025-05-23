using Discount.Services.Configurations;
using Discount.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Discount.Services.Discount
{
    public class CarrierTypeDiscountRule : IDiscountRule
    {
        private readonly IEnumerable<CarrierTypeDiscountConfig> _configs;

        public CarrierTypeDiscountRule(IOptions<DiscountRulesConfig> config)
        {
            _configs = config.Value.CarrierType;
        }

        public double Apply(PurchasedTicketData input)
        {
            if (string.IsNullOrEmpty(input.Type))
                return 0;

            var rule = _configs.FirstOrDefault(c =>
                string.Equals(c.Type, input.Type, StringComparison.OrdinalIgnoreCase));

            return rule?.DiscountPercent ?? 0;
        }
    }
}