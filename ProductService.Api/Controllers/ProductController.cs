using Microsoft.AspNetCore.Mvc;

namespace ProductService.Api.Controllers;

[Route("product")]
public class ProductController : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("adsd");
    }
}