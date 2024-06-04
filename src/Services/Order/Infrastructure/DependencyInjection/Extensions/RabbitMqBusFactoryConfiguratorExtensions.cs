using Contracts.Abstractions.Message;
using Contracts.Services.V1.Catalog.Ticket;
using Contracts.Services.V1.Identity.Customer;
using Infrastructure.Consumers;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        // Customer
        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerCreatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerCreated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerUpdatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerUpdated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerDeletedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerDeleted>(context);

        // Ticket
        cfg.ConfigureEventReceiveEndpoint<
            TicketConsumer.TicketCreatedConsumer,
            Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketCreated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            TicketConsumer.TicketDeletedConsumer,
            Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketDeleted>(context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
    where TConsumer : class, IConsumer
    where TEvent : class, IDomainEvent
    => bus.ReceiveEndpoint(
        queueName: $"order-service.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
        configureEndpoint: endpoint =>
        {
            endpoint.ConfigureConsumeTopology = false;
            endpoint.Bind<TEvent>();
            endpoint.ConfigureConsumer<TConsumer>(context);
        });
}