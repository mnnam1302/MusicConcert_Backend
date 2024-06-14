using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;

using Application.DependencyInjection.Extensions;
using Persistence.DependencyInjection.Extensions;
using Persistence.DependencyInjection.Options;
using API.DependencyInjection.Extensions;
using Infrastructure.DependencyInjection.Extensions;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "Order")
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

// Persistence
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddInterceptorPersistence();
builder.Services.AddRepositoryPersistence();
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));


// Infrastructure
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);
builder.Services.AddQuartzInfrastructure();
builder.Services.AddServiceInfrastructure(builder.Configuration);

// Middleware
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