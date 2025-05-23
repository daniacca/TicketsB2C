using Discount.Services.Discount;

namespace Discount.Services.Interfaces
{
    public interface IDiscountEngine
    {
        double ApplyAll(PurchasedTicketData input);
    }
}