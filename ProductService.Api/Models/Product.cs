using System.ComponentModel.DataAnnotations;

namespace ProductService.Api.Models;

/// <summary>
/// Simple product that we operate 
/// </summary>
public record Product
{
    /// <summary>
    /// Name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Name { get; init; }
    
    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; init; }
    
    /// <summary>
    /// Price
    /// </summary>
    [Required]
    public decimal Price { get; init; }
    
    /// <summary>
    /// Currency
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(3)]
    public string Currency { get; init; }
}