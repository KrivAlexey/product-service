using ProductService.Api;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApp(app);
app.Run();

static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ProductRepository>();

    var mvcBuilder = services.AddControllers();
    mvcBuilder.AddControllersAsServices();
}

static void ConfigureApp(WebApplication app)
{
    app.MapControllers();
}

/// <summary>
///  Entry point for the  WebApplicationFactory
/// </summary>
public partial class Program { }