using Unsolid.Models;
using Microsoft.AspNetCore.Mvc;
using Unsolid.Services.Abstractions;

namespace Unsolid.Controllers;

[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost("customers")]
    public IActionResult CreateCustomer([FromBody] CustomerCreateRequest request)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address ?? string.Empty,
            RegistrationDate = DateTime.Now,
            TotalOrders = 0,
            TotalSpent = 0,
            IsVip = false,
            MembershipType = "Regular"
        };

        customerService.AddCustomer(customer);
        return Created($"/customers/{customer.Id}", customer);
    }

    [HttpGet("customers/{id}")]
    public IActionResult GetCustomer(int id)
    {
        var customer = customerService.GetCustomer(id);
        if (customer == null)
            return NotFound(new { error = "Customer not found" });
        return Ok(customer);
    }
}