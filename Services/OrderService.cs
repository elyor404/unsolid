using Unsolid.Models;
using Unsolid.Services.Abstractions;

namespace Unsolid.Services
{
    public class OrderService : IOrderService
    {
        private static List<Order> Orders = [];
        private static int _orderIdCounter = 1;

        public void AddOrder(Order order)
        {
            order.Id = _orderIdCounter++;
            order.OrderDate = DateTime.Now;
            Orders.Add(order);
        }

        public Order? GetOrder(int id)
            => Orders.FirstOrDefault(o => o.Id == id);

        public List<Order> GetOrders()
            => [.. Orders];
    }
}