using Unsolid.Models;
using Unsolid.Services;
using Microsoft.AspNetCore.Mvc;

namespace Unsolid.Controllers;

[ApiController]
public class ShopController : ControllerBase
{
    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        var products = DataService.GetProducts();
        var result = products.Select(p => new
        {
            p.Id,
            p.Name,
            p.Price,
            p.Stock,
            p.Description,
            p.Category
        }).ToList();
        return Ok(result);
    }

    [HttpGet("products/{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = DataService.GetProduct(id);
        if (product == null)
        {
            return NotFound(new { error = "Product not found" });
        }
        if (!product.IsActive)
        {
            return BadRequest(new { error = "Product is not active" });
        }
        return Ok(new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.Description,
            product.Category,
            product.Discount
        });
    }

    [HttpPost("products")]
    public IActionResult CreateProduct([FromBody] Models.Product product)
    {
        var validation = BusinessService.ValidateProduct(product.Name, product.Price, product.Stock);
        if (validation != "OK")
        {
            return BadRequest(new { error = validation });
        }

        DataService.AddProduct(product);
        return Created($"/products/{product.Id}", product);
    }

    [HttpPost("products/{id}/discount")]
    public IActionResult ApplyDiscount(int id, [FromBody] Models.DiscountRequest request)
    {
        if (request.Percentage <= 0 || request.Percentage > 100)
        {
            return BadRequest(new { error = "Discount percentage must be between 1 and 100" });
        }

        var product = DataService.GetProduct(id);
        if (product == null)
        {
            return NotFound(new { error = "Product not found" });
        }

        product.Discount = request.Percentage;

        return Ok(new
        {
            message = "Discount applied successfully",
            productId = product.Id,
            discount = request.Percentage
        });
    }
}

