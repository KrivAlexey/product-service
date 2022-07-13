using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Dal;
using Product = ProductService.Api.Models.Product;

namespace ProductService.Api.Tests;

public class ApiIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private readonly JsonSerializerOptions _serializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    [OneTimeSetUp]
    public async Task Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => { }
            );
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory!.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        await dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE products");
    }

    [Test]
    public async Task ShouldNotFound_WhenEmptyRepository()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/product/product_not_found");

        Assert.False(response.IsSuccessStatusCode);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task ShouldGetProduct_WhenCreate()
    {
        var client = _factory.CreateClient();

        var productToCreate = new Product("product_1", "description", 1, "USD");

        var createResponse = await client.PostAsync("product", JsonContent.Create(productToCreate));
        Assert.True(createResponse.IsSuccessStatusCode);
        
        var createdProduct = JsonSerializer.Deserialize<Product>(await createResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(createdProduct, Is.EqualTo(productToCreate));

        var getResponse = await client.GetAsync($"product/{productToCreate.Name}");
        Assert.True(getResponse.IsSuccessStatusCode);
        
        var getProduct = JsonSerializer.Deserialize<Product>(await getResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(getProduct, Is.EqualTo(productToCreate));
    }
    
    [Test]
    public async Task ShouldConflict_WhenCreateWithSameName()
    {
        var client = _factory.CreateClient();

        var productToCreate = new Product("product_2", "description", 1, "USD");

        var createResponse = await client.PostAsync("product", JsonContent.Create(productToCreate));
        Assert.True(createResponse.IsSuccessStatusCode);
        
        var createdProduct = JsonSerializer.Deserialize<Product>(await createResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(createdProduct, Is.EqualTo(productToCreate));
        
        var productToCreateSameName = new Product("product_2", "description_2", 2, "USD");
        var createAgainResponse = await client.PostAsync("product", JsonContent.Create(productToCreateSameName));
        
        Assert.False(createAgainResponse.IsSuccessStatusCode);
        Assert.That(createAgainResponse.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task ShouldUpdateAndGetUpdated_AfterCreateProduct()
    {
        var client = _factory.CreateClient();

        var productToCreate = new Product("product_3", "description", 1, "USD");

        var createResponse = await client.PostAsync("product", JsonContent.Create(productToCreate));
        Assert.True(createResponse.IsSuccessStatusCode);
        
        var createdProduct = JsonSerializer.Deserialize<Product>(await createResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(createdProduct, Is.EqualTo(productToCreate));
        
        var productToUpdate = new Product("product_3", "description", 2, "USD");
        var updateResponse = await client.PutAsync($"product/{productToUpdate.Name}", JsonContent.Create(productToUpdate));
        Assert.True(updateResponse.IsSuccessStatusCode);
        
        var getResponse = await client.GetAsync($"product/{productToUpdate.Name}");
        Assert.True(getResponse.IsSuccessStatusCode);
        
        var getProduct = JsonSerializer.Deserialize<Product>(await getResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(getProduct, Is.EqualTo(productToUpdate));
    }

    [Test]
    public async Task ShouldNotFound_WhenUpdateUnknownProduct()
    {
        var client = _factory.CreateClient();

        var productToUpdate = new Product("product_4", "description", 2, "USD");
        var updateResponse = await client.PutAsync($"product/{productToUpdate.Name}", JsonContent.Create(productToUpdate));
        Assert.False(updateResponse.IsSuccessStatusCode);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task ShouldNotFound_AfterDeleteProduct()
    {
        var client = _factory.CreateClient();

        var productToCreate = new Product("product_5", "description", 1, "USD");

        var createResponse = await client.PostAsync("product", JsonContent.Create(productToCreate));
        Assert.True(createResponse.IsSuccessStatusCode);
        
        var createdProduct = JsonSerializer.Deserialize<Product>(await createResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(createdProduct, Is.EqualTo(productToCreate));
        
        var getResponse = await client.GetAsync($"product/{productToCreate.Name}");
        Assert.True(getResponse.IsSuccessStatusCode);
        
        var getProduct = JsonSerializer.Deserialize<Product>(await getResponse.Content.ReadAsStreamAsync(), _serializationOptions);
        Assert.That(getProduct, Is.EqualTo(productToCreate));

        var deleteResponse = await client.DeleteAsync($"product/{productToCreate.Name}");
        Assert.True(deleteResponse.IsSuccessStatusCode);
        
        var getAfterDeleteResponse = await client.GetAsync($"product/{productToCreate.Name}");
        Assert.That(getAfterDeleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}