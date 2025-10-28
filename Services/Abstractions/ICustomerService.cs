using Unsolid.Models;

namespace Unsolid.Services.Abstractions
{
    public interface ICustomerService
    {
        public Customer? GetCustomer(int id);
        public void AddCustomer(Customer customer);
    }
}