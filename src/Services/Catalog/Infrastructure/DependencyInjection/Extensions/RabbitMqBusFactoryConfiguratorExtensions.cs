using Contracts.Abstractions.Message;
using Infrastructure.Consumers.Events;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        // Organizaiton
        cfg.ConfigureEventReceiveEndpoint<
            OrganizationConsumer.OrganizationCreatedConsumer,

            Contracts.Services.V1.Identity.Organization.DomainEvent.OrganizationCreated>(context);
        cfg.ConfigureEventReceiveEndpoint<
            OrganizationConsumer.OrganizationDeletedConsumer,
            Contracts.Services.V1.Identity.Organization.DomainEvent.OrganizationDeleted>(context);
    
        // Order
        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.OrderCreatedConsumer,
            Contracts.Services.V1.Order.DomainEvent.OrderCreated>(context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
    where TConsumer : class, IConsumer
    where TEvent : class, IDomainEvent
    => bus.ReceiveEndpoint(
        queueName: $"catalog-service.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
        configureEndpoint: endpoint =>
        {
            endpoint.ConfigureConsumeTopology = false;
            endpoint.Bind<TEvent>();
            endpoint.ConfigureConsumer<TConsumer>(context);
        });
}