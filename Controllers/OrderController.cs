using Unsolid.Models;
using Unsolid.Services;
using Microsoft.AspNetCore.Mvc;

namespace Unsolid.Controllers;

[ApiController]
public class OrderController : ControllerBase
{
    [HttpGet("orders")]
    public IActionResult GetOrders()
    {
        var orders = DataService.GetOrders();
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
        var order = DataService.GetOrder(id);
        if (order == null)
        {
            return NotFound(new { error = "Order not found" });
        }
        return Ok(order);
    }

    [HttpPost("orders")]
    public IActionResult CreateOrder([FromBody] Models.OrderCreateRequest request)
    {
        if (request.CustomerId <= 0)
        {
            return BadRequest(new { error = "Customer ID is required" });
        }
        if (request.Items == null || request.Items.Count == 0)
        {
            return BadRequest(new { error = "Order must have at least one item" });
        }
        if (string.IsNullOrWhiteSpace(request.ShippingAddress))
        {
            return BadRequest(new { error = "Shipping address is required" });
        }

        var customer = DataService.GetCustomer(request.CustomerId);
        if (customer == null)
        {
            return NotFound(new { error = "Customer not found" });
        }

        decimal subtotal = 0;
        var orderItems = new List<Models.OrderItem>();

        foreach (var item in request.Items)
        {
            var product = DataService.GetProduct(item.ProductId);
            if (product == null)
            {
                return BadRequest(new { error = $"Product {item.ProductId} not found" });
            }
            if (product.Stock < item.Quantity)
            {
                return BadRequest(new { error = $"Insufficient stock for product {product.Name}" });
            }
            if (!product.IsActive)
            {
                return BadRequest(new { error = $"Product {product.Name} is not active" });
            }

            decimal unitPrice = product.Price * (1 - product.Discount / 100);
            decimal itemSubtotal = unitPrice * item.Quantity;
            subtotal += itemSubtotal;

            orderItems.Add(new Models.OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
                Subtotal = itemSubtotal
            });

            product.Stock -= item.Quantity;
        }

        decimal tax = BusinessService.CalculateTax(subtotal);
        decimal shipping = BusinessService.CalculateShipping(customer.IsVip, subtotal, orderItems.Count);

        decimal discountAmount = 0;
        if (customer.IsVip)
        {
            discountAmount = BusinessService.ApplyVipDiscount(subtotal, true);
        }

        decimal loyaltyBonus = BusinessService.ApplyLoyaltyBonus(subtotal, customer.TotalOrders);

        decimal total = subtotal + tax + shipping - discountAmount - loyaltyBonus;

        var order = new Models.Order
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

        DataService.AddOrder(order);
        customer.TotalOrders++;
        customer.TotalSpent += total;

        BusinessService.ProcessCustomerUpgrade(customer);

        return Created($"/orders/{order.Id}", order);
    }

    [HttpPost("orders/{id}/ship")]
    public IActionResult ShipOrder(int id)
    {
        var order = DataService.GetOrder(id);
        if (order == null)
        {
            return NotFound(new { error = "Order not found" });
        }
        if (order.Status != "Pending")
        {
            return BadRequest(new { error = $"Order is already {order.Status}" });
        }

        order.Status = "Shipped";
        order.ShippedDate = DateTime.Now;

        return Ok(new { message = "Order shipped successfully", order });
    }
}

