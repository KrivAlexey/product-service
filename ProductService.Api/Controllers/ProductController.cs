using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductService.Api.Models;
using ProductService.Api.Settings;

namespace ProductService.Api.Controllers;

/// <summary>
/// CRUD for products 
/// </summary>
[ApiController]
[Route("product")]
[ApiVersion("1")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ProductController : ControllerBase
{
    private readonly ProductRepository _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository">Repository of products</param>
    public ProductController(ProductRepository repository, IOptions<ProductServiceSettings> options)
    {
        _repository = repository;
    }

    /// <summary>
    /// Get product by name
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <response code="200">Product</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{productName}", Name = "Get")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public IActionResult Get([FromRoute]  [Required] [MaxLength(100)] string productName)
    {
        if (!_repository.Products.TryGetValue(productName, out var product))
        {
            return NotFound();
        }
        return Ok(product);
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="product">Product to create</param>
    /// <response code="201">Product create</response>
    /// <response code="409">Product with the same name already exist</response>
    [HttpPost(Name = "Create")]
    [ProducesResponseType(typeof(Product), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        if (_repository.Products.TryGetValue(product.Name, out _))
        {
            return Conflict();
        }

        _repository.Products[product.Name] = product;
        
        return CreatedAtAction(nameof(Get),new {productName = product.Name}, product);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="product">Product to update</param>
    /// <response code="204">Product updated</response>
    /// <response code="404">Product not found</response>
    /// <response code="400">Bad request</response>
    [HttpPut("{productName}", Name = "Update")]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public IActionResult UpdateProduct([FromRoute] [Required] [MaxLength(100)] string productName, [FromBody] Product product)
    {
        if (productName != product.Name)
        {
            return BadRequest();
        }
        
        if (!_repository.Products.TryGetValue(productName, out _))
        {
            return NotFound();
        }

        _repository.Products[productName] = product;
        return NoContent();
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productName">Product name to delete</param>
    /// <response code="204">Product updated</response>
    /// <response code="404">Product not found</response>
    [HttpDelete("{productName}", Name = "Delete")]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public IActionResult DeleteProduct([FromRoute] [Required] [MaxLength(100)] string productName)
    {
        if (!_repository.Products.TryGetValue(productName, out _))
        {
            return NotFound();
        }

        _repository.Products.Remove(productName);
        return NoContent();
    }
}