using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Api;
using ProductService.Api.Settings;
using ProductService.Dal;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureApp(app);
app.Run();

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddOptions<ProductServiceSettings>()
        .Bind(configuration.GetRequiredSection(nameof(ProductServiceSettings)))
        .ValidateDataAnnotations();
    services.AddSingleton<ProductRepository>();
    services.AddSwaggerGen(config =>
    {
        var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        var xmlDocExists = File.Exists(xmlPath);
        if (xmlDocExists)
        {
            config.IncludeXmlComments(xmlPath);
        }
    });

    services.AddDbContextPool<ProductDbContext>((provider, builder) =>
    {
        var options = provider.GetRequiredService<IOptions<ProductServiceSettings>>();
        builder.UseNpgsql(options.Value.ProductsDB).UseSnakeCaseNamingConvention();
    });

    var mvcBuilder = services.AddControllers();
    mvcBuilder.AddControllersAsServices();
}

static void ConfigureApp(WebApplication app)
{
    app.MapControllers();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
}

/// <summary>
///  Entry point for the  WebApplicationFactory
/// </summary>
public partial class Program { }