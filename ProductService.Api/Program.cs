var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApp(app);
app.Run();

static void ConfigureServices(IServiceCollection services)
{
    var mvcBuilder = services.AddControllers();
    mvcBuilder.AddControllersAsServices();
}

static void ConfigureApp(WebApplication app)
{
    app.MapControllers();
}