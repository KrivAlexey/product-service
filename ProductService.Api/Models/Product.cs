using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductService.Api.Models;

/// <summary>
/// Simple product that we operate 
/// </summary>
public record Product
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    /// <param name="currency"></param>
    public Product(string name, string? description, decimal price, string currency)
    {
        Name = name;
        Description = description;
        Price = price;
        Currency = currency;
    }

    /// <summary>
    /// Name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; }
    
    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; }
    
    /// <summary>
    /// Price
    /// </summary>
    [Required]
    public decimal Price { get; }
    
    /// <summary>
    /// Currency
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(3)]
    public string Currency { get; }
}