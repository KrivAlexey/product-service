using System.ComponentModel.DataAnnotations;
namespace ProductService.Dal;

/// <summary>
/// Simple product that we operate 
/// </summary>
public class Product
{
    [Key]
    public int Id { get; set; }
    
    /// <summary>
    /// Name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Price
    /// </summary>
    [Required]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Currency
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(3)]
    public string? Currency { get; set; }
    
    [Required]
    public DateTimeOffset CreatedOn { get; set; }
    
    public DateTimeOffset? RemovedOn { get; set; }
}