using Unsolid.Models;

namespace Unsolid.Services.Abstractions
{
    public interface IBusinessService
    {
        public decimal CalculateTax(decimal amount);
        public decimal CalculateShipping(bool isVip, decimal orderTotal, int itemCount);
        public decimal ApplyVipDiscount(decimal amount, bool isVip);
        public decimal ApplyLoyaltyBonus(decimal amount, int orderCount);
        public Customer? ProcessCustomerUpgrade(Customer customer);
    }
}