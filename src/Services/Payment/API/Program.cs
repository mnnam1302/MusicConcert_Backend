using Carter;
using Serilog;

using API.DependencyInjection.Extensions;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Persistence.DependencyInjection.Extensions;
using Application.DependencyInjection.Extensions;
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

// Persistence
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection("SqlServerRetryOptions"));
builder.Services.AddSqlServerPersistence(builder.Configuration);
builder.Services.AddInterceptorPersistence();
builder.Services.AddRepositoryPersistence();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapCarter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAPI();
}

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.Run();
