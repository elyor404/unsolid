using Unsolid.Models;

namespace Unsolid.Services.Abstractions
{
    public interface IOrderService
    {
        public Order? GetOrder(int id);
        public List<Order> GetOrders();
        public void AddOrder(Order order);
    }
}