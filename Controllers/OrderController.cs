using Unsolid.Models;
using Microsoft.AspNetCore.Mvc;
using Unsolid.Services.Abstractions;

namespace Unsolid.Controllers;
[ApiController]
public class OrderController(
    IOrderService orderService,
    ICustomerService customerService,
    IProductService productService,
    IBusinessService businessService) : ControllerBase
{
    [HttpGet("orders")]
    public IActionResult GetOrders()
    {
        var orders = orderService.GetOrders();
        var result = orders.Select(o => new
        {
            o.Id,
            o.CustomerId,
            o.Status,
            o.OrderDate,
            o.Total,
            Items = o.Items.Count
        }).ToList();
        return Ok(result);
    }

    [HttpGet("orders/{id}")]
    public IActionResult GetOrder(int id)
    {
        var order = orderService.GetOrder(id);
        if (order == null)
            return NotFound(new { error = "Order not found" });
        return Ok(order);
    }

    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] OrderCreateRequest request)
    {
        var customer = customerService.GetCustomer(request.CustomerId);
        if (customer == null)
        {
            return NotFound(new { error = "Customer not found" });
        }

        decimal subtotal = 0;
        var orderItems = new List<OrderItem>();

        foreach (var item in request.Items)
        {
            var product = productService.GetProduct(item.ProductId);
            if (product == null)
                return BadRequest(new { error = $"Product {item.ProductId} not found" });
            else if (product.Stock < item.Quantity)
                return BadRequest(new { error = $"Insufficient stock for product {product.Name}" });
            else if (!product.IsActive)
                return BadRequest(new { error = $"Product {product.Name} is not active" });

            decimal unitPrice = product.Price * (1 - product.Discount / 100);
            decimal itemSubtotal = unitPrice * item.Quantity;
            subtotal += itemSubtotal;

            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
                Subtotal = itemSubtotal
            });

            product.Stock -= item.Quantity;
        }

        decimal tax = businessService.CalculateTax(subtotal);
        decimal shipping = businessService.CalculateShipping(customer.IsVip, subtotal, orderItems.Count);

        decimal discountAmount = 0;
        if (customer.IsVip)
            discountAmount = businessService.ApplyVipDiscount(subtotal, true);

        decimal loyaltyBonus = businessService.ApplyLoyaltyBonus(subtotal, customer.TotalOrders);

        decimal total = subtotal + tax + shipping - discountAmount - loyaltyBonus;

        var order = new Order
        {
            Id = 0,
            CustomerId = request.CustomerId,
            Items = orderItems,
            Total = total,
            Tax = tax,
            Shipping = shipping,
            Status = "Pending",
            OrderDate = DateTime.Now,
            ShippedDate = null,
            ShippingAddress = request.ShippingAddress,
            DiscountAmount = discountAmount + loyaltyBonus
        };

        orderService.AddOrder(order);
        customer.TotalOrders++;
        customer.TotalSpent += total;

        businessService.ProcessCustomerUpgrade(customer);

        return Created($"/orders/{order.Id}", order);
    }

    [HttpPost("orders/{id}/ship")]
    public IActionResult ShipOrder(int id)
    {
        var order = orderService.GetOrder(id);
        if (order == null)
            return NotFound(new { error = "Order not found" });
        else if (order.Status != "Pending")
            return BadRequest(new { error = $"Order is already {order.Status}" });

        order.Status = "Shipped";
        order.ShippedDate = DateTime.Now;

        return Ok(new { message = "Order shipped successfully", order });
    }
}

