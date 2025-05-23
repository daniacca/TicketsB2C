using Discount.Services.Configurations;
using Discount.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Discount.Services.Discount
{
    public class DiscountEngine(IEnumerable<IDiscountRule> rules, IOptions<DiscountEngineConfig> engineConfig) : IDiscountEngine
    {
        private readonly IEnumerable<IDiscountRule> _rules = rules;
        private readonly DiscountEngineConfig _engineConfig = engineConfig.Value;

        public double ApplyAll(PurchasedTicketData input)
        {
            double discount = 0;
            var Stack = (double actual, double discount) => actual + discount;
            var Max = (double actual, double discount) => Math.Max(actual, discount);

            foreach (var rule in _rules)
            {
                var ruleDiscount = rule.Apply(input);
                discount = _engineConfig.Strategy switch
                {
                    DiscountApplyStrategy.Stackable => Stack(discount, ruleDiscount),
                    DiscountApplyStrategy.Maximum => Max(discount, ruleDiscount),
                    _ => throw new ArgumentOutOfRangeException(nameof(_engineConfig.Strategy), "Invalid discount type")
                };
            }

            return discount > 0 ? input.TotalCost * (1 - discount) : input.TotalCost;
        }
    }
}
