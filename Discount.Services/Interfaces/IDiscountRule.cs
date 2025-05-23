using Discount.Services.Discount;

namespace Discount.Services.Interfaces
{
    public interface IDiscountRule : IRule<PurchasedTicketData, double>
    {
    }
}
