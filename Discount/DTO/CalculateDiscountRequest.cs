namespace Discount.DTO;

public class CalculateDiscountRequest
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public double Total { get; set; }
    public string Type { get; set; } = string.Empty;
}
