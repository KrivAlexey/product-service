namespace ProductService.Api.Models;

/// <summary>
/// Mapping extensions
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Convert from dal models to api models
    /// </summary>
    /// <param name="product">Dal product model</param>
    /// <returns>Api product model</returns>
    public static Product ToApi(this Dal.Product product)
    {
        return new Product
        (
            product.Name!, 
            product.Description,
            product.Price,
            product.Currency!
        );
    }

    /// <summary>
    /// Convert from api model to dal model
    /// </summary>
    /// <param name="product">Api product model</param>
    /// <param name="createdOn">Created on date time</param>
    /// <returns>Dal product model</returns>
    public static Dal.Product ToDal(this Product product, DateTimeOffset createdOn)
    {
        return new Dal.Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Currency = product.Currency,
            CreatedOn = createdOn,
            RemovedOn = null
        };
    }
}