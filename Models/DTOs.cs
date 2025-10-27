namespace Unsolid.Models;

public class OrderCreateRequest
{
    public int CustomerId { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    public string ShippingAddress { get; set; } = string.Empty;
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CustomerCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class DiscountRequest
{
    public decimal Percentage { get; set; }
}

