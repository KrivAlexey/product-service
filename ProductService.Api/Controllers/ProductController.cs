using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Models;

namespace ProductService.Api.Controllers;

[Route("product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductRepository _repository;

    public ProductController(ProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string productName)
    {
        if (!_repository.Products.TryGetValue(productName, out var product))
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        if (!_repository.Products.TryGetValue(product.Name, out _))
        {
            return Conflict();
        }

        _repository.Products[product.Name] = product;
        
        return CreatedAtAction(nameof(Get), product.Name, product);
    }

    [HttpPut]
    public IActionResult UpdateProduct([FromBody] Product product)
    {
        if (!_repository.Products.TryGetValue(product.Name, out _))
        {
            return NotFound();
        }

        _repository.Products[product.Name] = product;
        return NoContent();
    }

    [HttpDelete]
    public IActionResult DeleteProduct([FromQuery] string productName)
    {
        if (!_repository.Products.TryGetValue(productName, out _))
        {
            return NotFound();
        }

        _repository.Products.Remove(productName);
        return NoContent();
    }
}