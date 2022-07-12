using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedAutoPropertyAccessor.Global

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
    public string? ProductsDb { get; set; }
}