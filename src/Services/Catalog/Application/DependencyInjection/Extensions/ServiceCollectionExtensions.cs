using Application.Behaviors;
using Application.Mapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(Contracts.AssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }

    public static IServiceCollection AddAutoMapperApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceProfile));

        return services;
    }
}