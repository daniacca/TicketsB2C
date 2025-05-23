
using Discount.Services.Configurations;
using Discount.Services.Discount;
using Microsoft.Extensions.Options;

namespace Discount.Services.Tests
{
    public class DiscountEngineTests
    {
        [Fact]
        public void ApplyAll_ShouldReturnDiscountedTotal_WhenValidInput()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 2, DiscountPercent = 0.10 }
                },
                CarrierType = new List<CarrierTypeDiscountConfig>
                {
                    new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.05 }
                }
            };
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = DiscountApplyStrategy.Stackable
            };

            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);

            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);

            // Crea il motore di sconto con le regole
            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);

            // Prepara i dati di input
            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 2,
                Price: 100,
                Type: "Bus",
                TotalCost: 200
            );

            // Act
            var result = discountEngine.ApplyAll(purchasedTicketData);

            // Assert
            Assert.Equal(170, result, precision: 2); // total discount of 10% + 5% = 15%
        }

        [Fact]
        public void ApplyAll_ShouldReturnOriginalTotal_WhenNoDiscounts()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = [],
                CarrierType = []
            };
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = DiscountApplyStrategy.Stackable
            };
            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);
            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);
            
            // Crea il motore di sconto con le regole
            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);
            
            // Prepara i dati di input
            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 1,
                Price: 100,
                Type: "Train",
                TotalCost: 100
            );
            
            // Act
            var result = discountEngine.ApplyAll(purchasedTicketData);
            
            // Assert
            Assert.Equal(100, result); // no discounts applied
        }

        [Fact]
        public void ApplyAll_ShouldReturnMaximumDiscount_WhenMaxStrategy()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = new List<QuantityDiscountConfig>
                {
                    new QuantityDiscountConfig { MinQuantity = 2, DiscountPercent = 0.10 }
                },
                CarrierType = new List<CarrierTypeDiscountConfig>
                {
                    new CarrierTypeDiscountConfig { Type = "Bus", DiscountPercent = 0.05 }
                }
            };
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = DiscountApplyStrategy.Maximum
            };

            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);
            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);
            
            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);
            
            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 2,
                Price: 100,
                Type: "Bus",
                TotalCost: 200
            );
            
            // Act
            var result = discountEngine.ApplyAll(purchasedTicketData);
            
            // Assert
            Assert.Equal(180, result); // max discount of 10%
        }

        [Fact]
        public void ApplyAll_ShouldThrowException_WhenInvalidStrategy()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = [],
                CarrierType = []
            };
            
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = (DiscountApplyStrategy)999 // Invalid strategy
            };
            
            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);
            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);

            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);

            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 1,
                Price: 100,
                Type: "Train",
                TotalCost: 100
            );

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => discountEngine.ApplyAll(purchasedTicketData));
        }

        [Fact]
        public void ApplyAll_ShouldReturnZero_WhenNoDiscountsAndZeroTotal()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = [],
                CarrierType = []
            };
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = DiscountApplyStrategy.Stackable
            };
            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);
            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);
            
            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);
            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 0,
                Price: 0,
                Type: "Train",
                TotalCost: 0
            );
            
            // Act
            var result = discountEngine.ApplyAll(purchasedTicketData);
            
            // Assert
            Assert.Equal(0, result); // no discounts applied
        }

        [Fact]
        public void ApplyAll_ShouldThrowsException_WhenNegativeTotal()
        {
            // Arrange: configura le regole di sconto
            var ruleConfig = new DiscountRulesConfig
            {
                Quantity = [],
                CarrierType = []
            };
            var engineConfig = new DiscountEngineConfig
            {
                Strategy = DiscountApplyStrategy.Stackable
            };
            var ruleOption = Options.Create(ruleConfig);
            var engineOption = Options.Create(engineConfig);
            var quantityRule = new QuantityDiscountRule(ruleOption);
            var carrierTypeRule = new CarrierTypeDiscountRule(ruleOption);

            var discountEngine = new DiscountEngine([quantityRule, carrierTypeRule], engineOption);
            var purchasedTicketData = new PurchasedTicketData(
                Quantity: 1,
                Price: 100,
                Type: "Train",
                TotalCost: -100
            );

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => discountEngine.ApplyAll(purchasedTicketData));
        }
    }
}
