using Carter;
using Serilog;
using Application.DependencyInjection.Extensions;
using Infrastructure.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "Authorization")
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Carter
builder.Services.AddCarter();


// Application
builder.Services.AddMediatRApplication();

// Infrastructure
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapCarter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

public partial class Program { }
