using Contracts.Abstractions.Message;
using Infrastructure.Consumers;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        // Identity
        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerCreatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerCreated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerUpdatedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerUpdated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            CustomerConsumer.CustomerDeletedConsumer,
            Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerDeleted>(context);

        // Catalog
        cfg.ConfigureEventReceiveEndpoint<
            TicketConsumer.TicketCreatedConsumer,
            Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketCreated>(context);

        cfg.ConfigureEventReceiveEndpoint<
            TicketConsumer.TicketDeletedConsumer,
            Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketDeleted>(context);

        // Reverse UnitInStocker from 'Catalog Service'
        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.StockReversedConsumer,
            Contracts.Services.V1.Order.DomainEvent.StockReversed>(context);

        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.StockReversedFailedConsumer,
            Contracts.Services.V1.Order.DomainEvent.StockReversedFailed>(context);

        // Payment
        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.PaymentProcessedConsumer,
            Contracts.Services.V1.Order.DomainEvent.PaymentProcessed>(context);

        cfg.ConfigureEventReceiveEndpoint<
            OrderConsumer.PaymentProcessedFailedConsumer,
            Contracts.Services.V1.Order.DomainEvent.PaymentProcessedFailed>(context);
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