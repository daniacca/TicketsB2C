namespace Discount.Services.Configurations;

public class QuantityDiscountConfig
{
    public int MinQuantity { get; set; }
    public double DiscountPercent { get; set; }
}

public class CarrierTypeDiscountConfig
{
    public string Type { get; set; } = string.Empty;
    public double DiscountPercent { get; set; }
}

public class DiscountRulesConfig
{
    public List<QuantityDiscountConfig> Quantity { get; set; } = new();
    public List<CarrierTypeDiscountConfig> CarrierType { get; set; } = new();
}