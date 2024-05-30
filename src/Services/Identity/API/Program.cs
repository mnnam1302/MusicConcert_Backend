using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;
using API.DependencyInjection.Extensions;
using Persistence.DependencyInjection.Extensions;
using Application.DependencyInjection.Extensions;
using Infrastructure.DependencyInjection.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "Identity")
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Carter
builder.Services.AddCarter();

// Swagger
builder.Services
    .AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwaggerAPI();

// API versioning
builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Application
builder.Services.AddMediatRApplication();
builder.Services.AddAutoMapperApplication();

// Persistence
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection("SqlServerRetryOptions"));
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddInterceptorPersistence();
builder.Services.AddRepositoryPersistence();

// Infrastructure
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);
builder.Services.AddQuartzInfrastructure();
builder.Services.AddServicesInfrastructure();
builder.Services.AddRedisInfrastructure(builder.Configuration);

// Midlleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Using Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapCarter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAPI();
}

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

try
{
    await app.RunAsync();
    Log.Information("Stop cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Program
{ }