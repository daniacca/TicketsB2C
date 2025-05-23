using Discount.Services.Configurations;
using Discount.Services.Discount;
using Microsoft.Extensions.Options;

namespace Discount.Services.Tests;

public class CarrierTypeDiscountRuleTests
{
    [Fact]
    public void Apply_WithMatchingCarrierType_AppliesCorrectDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>
            {
                new CarrierTypeDiscountConfig { Type = "Train", DiscountPercent = 0.10 }
            }
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);

        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: "Train");

        // Act
        var discountedTotal = rule.Apply(input);

        // Assert
        Assert.Equal(0.10, discountedTotal); // 10% discount for Train
    }

    [Fact]
    public void Apply_WithNonMatchingCarrierType_NoDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>
            {
                new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.10 }
            }
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);

        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: "Train");

        // Act
        var discountedTotal = rule.Apply(input);

        // Assert
        Assert.Equal(0, discountedTotal); // Only apply Discount for bus, no discount for Train
    }

    [Fact]
    public void Apply_WithEmptyCarrierType_NoDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>
            {
                new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.10 }
            }
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);
        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: string.Empty);
        
        // Act
        var discountedTotal = rule.Apply(input);
        
        // Assert
        Assert.Equal(0, discountedTotal); // No discount for empty type
    }

    [Fact]
    public void Apply_WithNullCarrierType_NoDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>
            {
                new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.10 }
            }
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);
        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: null);

        // Act
        var discountedTotal = rule.Apply(input);

        // Assert
        Assert.Equal(0, discountedTotal); // No discount for null type
    }

    [Fact]
    public void Apply_WithNoCarrierTypeDiscounts_NoDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>()
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);
        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: "Train");
        
        // Act
        var discountedTotal = rule.Apply(input);
        
        // Assert
        Assert.Equal(0, discountedTotal); // No discounts defined
    }

    [Fact]
    public void Apply_WithMultipleCarrierTypeDiscounts_AppliesCorrectDiscount()
    {
        // Arrange
        var config = new DiscountRulesConfig
        {
            CarrierType = new List<CarrierTypeDiscountConfig>
            {
                new CarrierTypeDiscountConfig { Type = "Train", DiscountPercent = 0.10 },
                new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.15 }
            }
        };
        var options = Options.Create(config);
        var rule = new CarrierTypeDiscountRule(options);
        var input = new PurchasedTicketData(Quantity: 2, Price: 50, TotalCost: 100, Type: "Bus");
        
        // Act
        var discountedTotal = rule.Apply(input);
        
        // Assert
        Assert.Equal(0.15, discountedTotal); // 15% discount for Bus
    }
}

