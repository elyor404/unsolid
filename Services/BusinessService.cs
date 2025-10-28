using Unsolid.Models;
using Unsolid.Services.Abstractions;

namespace Unsolid.Services;

public class BusinessService : IBusinessService
{
    public const decimal TaxRate = 0.08m;
    public const decimal ShippingFlatRate = 10.00m;
    public const decimal VipDiscountPercent = 0.10m;
    public const decimal LoyaltyBonusPercent = 0.05m;
    public const decimal MinOrderForFreeShipping = 100m;
    public const int VipThresholdOrders = 10;
    public const int MinOrderItemsForExtraShipping = 5;

    public decimal CalculateTax(decimal amount)
    {
        return amount * TaxRate;
    }

    public decimal CalculateShipping(bool isVip, decimal orderTotal, int itemCount)
    {
        if (isVip || orderTotal >= MinOrderForFreeShipping) return 0;
        else if (itemCount > MinOrderItemsForExtraShipping)
            return ShippingFlatRate * 1.5m;

        return ShippingFlatRate;
    }

    public decimal ApplyVipDiscount(decimal amount, bool isVip)
    {
        return isVip ? amount * VipDiscountPercent : 0;
    }

    public decimal ApplyLoyaltyBonus(decimal amount, int orderCount)
    {
        if (orderCount > 5)
            return amount * LoyaltyBonusPercent;
        return 0;
    }

    public Customer? ProcessCustomerUpgrade(Customer customer)
    {
        if (customer.TotalOrders >= VipThresholdOrders && !customer.IsVip)
        {
            customer.IsVip = true;
            customer.MembershipType = "Gold";
        }
        return customer;
    }
}

