using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace ApiGateway.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYarpReverseProxyApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        return services;
    }

    public static IServiceCollection AddCorsAPI(this IServiceCollection services)
    {
        //services.AddCors(options =>
        //{
        //    options.AddPolicy("mypolicy", builder =>
        //    {
        //        //builder.WithOrigins("http://localhost:5173/")
        //        //    .AllowAnyMethod()
        //        //    .AllowAnyHeader();

        //        builder.AllowAnyOrigin()
        //                .AllowAnyMethod()
        //                .AllowAnyHeader();
        //    });
        //});

        services.AddCors();

        return services;
    }
}
