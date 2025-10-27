using Unsolid.Models;

namespace Unsolid.Services;

public class DataService
{
    private static List<Product> _products = new List<Product>();
    private static List<Order> _orders = new List<Order>();
    private static List<Customer> _customers = new List<Customer>();
    
    private static int _productIdCounter = 1;
    private static int _orderIdCounter = 1;
    private static int _customerIdCounter = 1;

    public static void InitializeData()
    {
        _products.Add(new Models.Product
        {
            Id = _productIdCounter++,
            Name = "Laptop Pro",
            Price = 1299.99m,
            Stock = 50,
            IsActive = true,
            Description = "High-performance laptop",
            CreatedAt = DateTime.Now,
            Discount = 0,
            Category = "Electronics"
        });

        _products.Add(new Models.Product
        {
            Id = _productIdCounter++,
            Name = "Wireless Mouse",
            Price = 29.99m,
            Stock = 200,
            IsActive = true,
            Description = "Ergonomic wireless mouse",
            CreatedAt = DateTime.Now,
            Discount = 0,
            Category = "Electronics"
        });

        _products.Add(new Models.Product
        {
            Id = _productIdCounter++,
            Name = "Office Chair",
            Price = 199.99m,
            Stock = 75,
            IsActive = true,
            Description = "Comfortable office chair",
            CreatedAt = DateTime.Now,
            Discount = 0,
            Category = "Furniture"
        });

        _customers.Add(new Models.Customer
        {
            Id = _customerIdCounter++,
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

        _customers.Add(new Models.Customer
        {
            Id = _customerIdCounter++,
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

    public static Product? GetProduct(int id) => _products.FirstOrDefault(p => p.Id == id);
    public static Customer? GetCustomer(int id) => _customers.FirstOrDefault(c => c.Id == id);
    public static Order? GetOrder(int id) => _orders.FirstOrDefault(o => o.Id == id);
    
    public static List<Product> GetProducts() => _products.Where(p => p.IsActive).ToList();
    public static List<Order> GetOrders() => _orders.ToList();
    
    public static void AddProduct(Product product)
    {
        product.Id = _productIdCounter++;
        product.CreatedAt = DateTime.Now;
        product.Discount = 0;
        _products.Add(product);
    }

    public static void AddCustomer(Customer customer)
    {
        customer.Id = _customerIdCounter++;
        customer.RegistrationDate = DateTime.Now;
        _customers.Add(customer);
    }

    public static void AddOrder(Order order)
    {
        order.Id = _orderIdCounter++;
        order.OrderDate = DateTime.Now;
        _orders.Add(order);
    }
}

