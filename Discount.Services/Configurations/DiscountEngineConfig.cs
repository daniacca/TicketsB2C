namespace Discount.Services.Configurations;

public enum DiscountApplyStrategy
{
    Stackable = 1,
    Maximum = 2,
}

public class DiscountEngineConfig 
{
    public DiscountApplyStrategy Strategy { get; set; } = DiscountApplyStrategy.Stackable;
}
