using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ProductService.Api.Tests;

public class ApiIntegrationTests
{

    [Test]
    public async Task ShouldNotFound_WhenEmptyRepository()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        
        var response = await client.GetAsync("/product/a");
        
        Assert.False(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}