using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
            //.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
            //.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>));

        return services;
    }
}