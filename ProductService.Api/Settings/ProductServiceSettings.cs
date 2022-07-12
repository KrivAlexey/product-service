using System.ComponentModel.DataAnnotations;

namespace ProductService.Api.Settings;

/// <summary>
/// Setting for ProductService
/// </summary>
public class ProductServiceSettings
{
    /// <summary>
    /// Db connection string
    /// </summary>
    [Required]
    public string? ProductsDB { get; set; }
}