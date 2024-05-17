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
}
