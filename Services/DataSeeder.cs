using Unsolid.Services.Abstractions;

namespace Unsolid.Services
{
    public class DataSeeder(
        IProductService productService,
        ICustomerService customerService) : IDataSeeder
    {
        public void InitializeData()
        {
            productService.AddProduct(new Models.Product
            {
                Name = "Laptop Pro",
                Price = 1299.99m,
                Stock = 50,
                IsActive = true,
                Description = "High-performance laptop",
                CreatedAt = DateTime.Now,
                Discount = 0,
                Category = "Electronics"
            });

            productService.AddProduct(new Models.Product
            {
                Name = "Wireless Mouse",
                Price = 29.99m,
                Stock = 200,
                IsActive = true,
                Description = "Ergonomic wireless mouse",
                CreatedAt = DateTime.Now,
                Discount = 0,
                Category = "Electronics"
            });

            productService.AddProduct(new Models.Product
            {
                Name = "Office Chair",
                Price = 199.99m,
                Stock = 75,
                IsActive = true,
                Description = "Comfortable office chair",
                CreatedAt = DateTime.Now,
                Discount = 0,
                Category = "Furniture"
            });

            customerService.AddCustomer(new Models.Customer
            {
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "555-0101",
                Address = "123 Main St",
                RegistrationDate = DateTime.Now.AddMonths(-6),
                TotalOrders = 5,
                TotalSpent = 2345.50m,
                IsVip = true,
                MembershipType = "Gold"
            });

            customerService.AddCustomer(new Models.Customer
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Phone = "555-0102",
                Address = "456 Oak Ave",
                RegistrationDate = DateTime.Now.AddMonths(-2),
                TotalOrders = 2,
                TotalSpent = 450.00m,
                IsVip = false,
                MembershipType = "Regular"
            });
        }
    }
}