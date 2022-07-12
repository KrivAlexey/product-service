using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Models;
using ProductService.Dal;
using Product = ProductService.Api.Models.Product;

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
    private readonly ProductDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">Repository of products</param>
    public ProductController(ProductDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get product by name
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="token">CancellationToken</param>
    /// <response code="200">Product</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{productName}", Name = "Get")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> Get([FromRoute] [Required] [MaxLength(100)] string productName, CancellationToken token)
    {
        var dbProduct = await _dbContext.Products
            .Where(p => p.Name == productName && p.RemovedOn == null)
            .SingleOrDefaultAsync(token);
        
        if (dbProduct is null)
        {
            return NotFound();
        }
        
        return Ok(dbProduct.ToApi());
    }

    /// <summary>
    /// Create product
    /// </summary>
    /// <param name="product">Product to create</param>
    /// <param name="token">CancellationToken</param>
    /// <response code="201">Product create</response>
    /// <response code="409">Product with the same name already exist</response>
    [HttpPost(Name = "Create")]
    [ProducesResponseType(typeof(Product), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    public async Task<IActionResult> CreateProduct([FromBody] Product product, CancellationToken token)
    {
        var dbProduct = await _dbContext.Products
            .Where(dbProduct => dbProduct.Name == product.Name)
            .SingleOrDefaultAsync(token);
        if (dbProduct is not null)
        {
            return Conflict();
        }

        _dbContext.Products.Add(product.ToDal(DateTimeOffset.UtcNow));
        try
        {
            await _dbContext.SaveChangesAsync(token);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict();
        }
        
        return CreatedAtAction(nameof(Get),new {productName = product.Name}, product);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="product">Product to update</param>
    /// <param name="token">CancellationToken</param>
    /// <response code="204">Product updated</response>
    /// <response code="404">Product not found</response>
    /// <response code="400">Bad request</response>
    [HttpPut("{productName}", Name = "Update")]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> UpdateProduct(
        [FromRoute] [Required] [MaxLength(100)] string productName, 
        [FromBody] Product product,
        CancellationToken token)
    {
        if (productName != product.Name)
        {
            return BadRequest();
        }
        
        var dbProduct = await _dbContext.Products
            .Where(dbProduct => dbProduct.Name == product.Name)
            .SingleOrDefaultAsync(token);
        
        if (dbProduct is null)
        {
            return NotFound();
        }

        dbProduct.Description = product.Description;
        dbProduct.Price = product.Price;
        dbProduct.Currency = product.Currency;

        await _dbContext.SaveChangesAsync(token);
        
        return NoContent();
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productName">Product name to delete</param>
    /// <param name="token">CancellationToken</param>
    /// <response code="204">Product updated</response>
    /// <response code="404">Product not found</response>
    [HttpDelete("{productName}", Name = "Delete")]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] [Required] [MaxLength(100)] string productName,
        CancellationToken token)
    {
        var dbProduct = await _dbContext.Products
            .Where(dbProduct => dbProduct.Name == productName)
            .SingleOrDefaultAsync(token); 
        
        if (dbProduct is null)
        {
            return NotFound();
        }

        dbProduct.RemovedOn = DateTimeOffset.UtcNow;
        return NoContent();
    }
}