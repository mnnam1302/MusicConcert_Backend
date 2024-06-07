using Contracts.Abstractions.Message;
using Contracts.Services.V1.Identity.Customer;
using Infrastructure.Consumers;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        // Customer Service
        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerCreatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerCreated>(context);
        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerUpdatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerUpdated>(context);
        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerDeletedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerDeleted>(context);

        // Order Service
        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.OrderValidatedConsumer,
            Contracts.Services.V1.Order.DomainEvent.OrderValidated>(context);
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