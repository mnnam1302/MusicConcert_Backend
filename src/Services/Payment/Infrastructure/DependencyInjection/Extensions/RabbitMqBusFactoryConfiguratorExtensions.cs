using Contracts.Abstractions.Message;
using Contracts.Services.V1.Identity.Customer;
using Infrastructure.Consumers;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ConfigureEventReceiveEndpoint<CustomerConsumer.CustomerCreatedConsumer, DomainEvent.CustomerCreated>(context);
        cfg.ConfigureEventReceiveEndpoint<CustomerConsumer.CustomerUpdatedConsumer, DomainEvent.CustomerUpdated>(context);
        cfg.ConfigureEventReceiveEndpoint<CustomerConsumer.CustomerDeletedConsumer, DomainEvent.CustomerDeleted>(context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
    where TConsumer : class, IConsumer
    where TEvent : class, IDomainEvent
    => bus.ReceiveEndpoint(
        queueName: $"payment-service.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
        configureEndpoint: endpoint =>
        {
            endpoint.ConfigureConsumeTopology = false;
            endpoint.Bind<TEvent>();
            endpoint.ConfigureConsumer<TConsumer>(context);
        });
}