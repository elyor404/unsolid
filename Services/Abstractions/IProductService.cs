using Unsolid.Models;

namespace Unsolid.Services.Abstractions
{
    public interface IProductService
    {
        public Product? GetProduct(int id);
        public List<Product> GetProducts();
        public void AddProduct(Product product);
    }
}