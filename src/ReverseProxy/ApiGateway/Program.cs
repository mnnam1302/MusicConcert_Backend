using ApiGateway.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();


// Yarp Reverse Proxy
builder.Services.AddYarpReverseProxyApiGateway(builder.Configuration);

builder.Services.AddCorsAPI();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.MapReverseProxy();

try
{
await app.RunAsync();
Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
await app.StopAsync();
}
finally
{
Log.CloseAndFlush();
await app.DisposeAsync();
}

public partial class Program { }
