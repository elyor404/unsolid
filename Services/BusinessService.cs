using Unsolid.Models;

namespace Unsolid.Services;

public class BusinessService
{
    public const decimal TaxRate = 0.08m;
    public const decimal ShippingFlatRate = 10.00m;
    public const decimal VipDiscountPercent = 0.10m;
    public const decimal LoyaltyBonusPercent = 0.05m;
    public const decimal MinOrderForFreeShipping = 100m;
    public const int VipThresholdOrders = 10;
    public const int MinOrderItemsForExtraShipping = 5;

    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return false;
        if (!email.Contains("@")) return false;
        if (!email.Contains(".")) return false;
        return true;
    }

    public static bool ValidatePhone(string phone)
    {
        if (string.IsNullOrEmpty(phone)) return false;
        if (phone.Length < 10) return false;
        if (!phone.Any(char.IsDigit)) return false;
        return true;
    }

    public static decimal CalculateTax(decimal amount)
    {
        return amount * TaxRate;
    }

    public static decimal CalculateShipping(bool isVip, decimal orderTotal, int itemCount)
    {
        if (isVip) return 0;
        if (orderTotal >= MinOrderForFreeShipping) return 0;
        if (itemCount > MinOrderItemsForExtraShipping)
        {
            return ShippingFlatRate * 1.5m;
        }
        return ShippingFlatRate;
    }

    public static decimal ApplyVipDiscount(decimal amount, bool isVip)
    {
        return isVip ? amount * VipDiscountPercent : 0;
    }

    public static decimal ApplyLoyaltyBonus(decimal amount, int orderCount)
    {
        if (orderCount > 5)
        {
            return amount * LoyaltyBonusPercent;
        }
        return 0;
    }

    public static string ValidateProduct(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Product name is required";
        if (name.Length < 3)
            return "Product name must be at least 3 characters";
        if (price <= 0)
            return "Price must be greater than zero";
        if (price > 100000)
            return "Price cannot exceed $100,000";
        if (stock < 0)
            return "Stock cannot be negative";
        return "OK";
    }

    public static Customer? ProcessCustomerUpgrade(Customer customer)
    {
        if (customer.TotalOrders >= VipThresholdOrders && !customer.IsVip)
        {
            customer.IsVip = true;
            customer.MembershipType = "Gold";
        }
        return customer;
    }
}

