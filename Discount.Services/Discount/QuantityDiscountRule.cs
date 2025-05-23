using Discount.Services.Configurations;
using Discount.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Discount.Services.Discount
{
    public class QuantityDiscountRule : IDiscountRule
    {
        private readonly IEnumerable<QuantityDiscountConfig> _configs;

        public QuantityDiscountRule(IOptions<DiscountRulesConfig> configs)
        {
            _configs = configs.Value.Quantity;
        }

        public double Apply(PurchasedTicketData input)
        {
            var applicable = _configs
                .Where(c => input.Quantity >= c.MinQuantity)
                .OrderByDescending(c => c.MinQuantity)
                .FirstOrDefault();

            return applicable?.DiscountPercent ?? 0.0;
        }
    }
}
