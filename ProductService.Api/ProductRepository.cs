using ProductService.Api.Models;

namespace ProductService.Api;

public class ProductRepository
{
    public Dictionary<string, Product> Products = new();
}