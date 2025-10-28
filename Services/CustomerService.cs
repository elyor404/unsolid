using Unsolid.Models;
using Unsolid.Services.Abstractions;

namespace Unsolid.Services
{
    public class CustomerService : ICustomerService
    {
        private static List<Customer> Customers = [];
        private static int CustomerIdCounter = 1;

        public void AddCustomer(Customer customer)
        {
            customer.Id = CustomerIdCounter++;
            customer.RegistrationDate = DateTime.Now;
            Customers.Add(customer);
        }
        public Customer? GetCustomer(int id)
            => Customers.FirstOrDefault(c => c.Id == id);
    }
}