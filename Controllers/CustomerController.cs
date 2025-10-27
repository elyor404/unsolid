using Unsolid.Models;
using Unsolid.Services;
using Microsoft.AspNetCore.Mvc;

namespace Unsolid.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    [HttpPost("customers")]
    public IActionResult CreateCustomer([FromBody] Models.CustomerCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { error = "Name is required" });
        }
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(new { error = "Email is required" });
        }
        if (!BusinessService.ValidateEmail(request.Email))
        {
            return BadRequest(new { error = "Invalid email format" });
        }
        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            return BadRequest(new { error = "Phone is required" });
        }
        if (!BusinessService.ValidatePhone(request.Phone))
        {
            return BadRequest(new { error = "Invalid phone format" });
        }

        var customer = new Models.Customer
        {
            Id = 0,
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

        DataService.AddCustomer(customer);
        return Created($"/customers/{customer.Id}", customer);
    }

    [HttpGet("customers/{id}")]
    public IActionResult GetCustomer(int id)
    {
        var customer = DataService.GetCustomer(id);
        if (customer == null)
        {
            return NotFound(new { error = "Customer not found" });
        }
        return Ok(customer);
    }
}

