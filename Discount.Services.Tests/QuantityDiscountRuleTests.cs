using Discount.Services.Configurations;
using Discount.Services.Discount;
using Microsoft.Extensions.Options;

namespace Discount.Services.Tests
{
    public class QuantityDiscountRuleTests
    {
        [Fact]
        public void Apply_WithMatchingQuantity_AppliesCorrectDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 5, DiscountPercent = 0.10 }
                }
            };

            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: 7, Price: 50, TotalCost: 350, Type: "Train");
            
            // Act
            var discountedTotal = rule.Apply(input);
            
            // Assert
            Assert.Equal(0.10, discountedTotal); // 10% discount for quantity more than 5
        }

        [Fact]
        public void Apply_WithNonMatchingQuantity_NoDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 5, DiscountPercent = 0.10 }
                }
            };
            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: 3, Price: 50, TotalCost: 150, Type: "Train");

            // Act
            var discountedTotal = rule.Apply(input);

            // Assert
            Assert.Equal(0, discountedTotal); // No discount for quantity less than 5
        }

        [Fact]
        public void Apply_WithMultipleDiscounts_AppliesHighestDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 5, DiscountPercent = 0.10 },
                    new QuantityDiscountConfig { MinQuantity = 10, DiscountPercent = 0.20 }
                }
            };
            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: 12, Price: 50, TotalCost: 600, Type: "Train");
            
            // Act
            var discountedTotal = rule.Apply(input);
            
            // Assert
            Assert.Equal(0.20, discountedTotal); // 20% discount for quantity more than 10
        }

        [Fact]
        public void Apply_WithNoDiscounts_NoDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>()
            };
            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: "Train");

            // Act
            var discountedTotal = rule.Apply(input);

            // Assert
            Assert.Equal(0, discountedTotal); // No discounts defined
        }

        [Fact]
        public void Apply_WithZeroQuantity_NoDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 5, DiscountPercent = 0.10 }
                }
            };
            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: 0, Price: 50, TotalCost: 0, Type: "Train");
            
            // Act
            var discountedTotal = rule.Apply(input);
            
            // Assert
            Assert.Equal(0, discountedTotal); // No discount for zero quantity
        }

        [Fact]
        public void Apply_WithNegativeQuantity_NoDiscount()
        {
            // Arrange
            var config = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 5, DiscountPercent = 0.10 }
                }
            };
            var options = Options.Create(config);
            var rule = new QuantityDiscountRule(options);
            var input = new PurchasedTicketData(Quantity: -3, Price: 50, TotalCost: -150, Type: "Train");

            // Act
            var discountedTotal = rule.Apply(input);

            // Assert
            Assert.Equal(0, discountedTotal); // No discount for negative quantity
        }
    }
}
