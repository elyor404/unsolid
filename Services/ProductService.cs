using Unsolid.Models;
using Unsolid.Services.Abstractions;

namespace Unsolid.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> Products = [];
        private static int ProductIdCounter = 1;

        public void AddProduct(Product product)
        {
            product.Id = ProductIdCounter++;
            product.CreatedAt = DateTime.Now;
            product.Discount = 0;
            Products.Add(product);
        }
        public Product? GetProduct(int id)
            => Products.FirstOrDefault(p => p.Id == id);

        public List<Product> GetProducts()
            => [.. Products.Where(p => p.IsActive)];
    }
}