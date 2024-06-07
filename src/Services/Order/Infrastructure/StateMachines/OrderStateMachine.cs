using Contracts.Services.V1.Order;
using Infrastructure.StateMachines;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Infrastructure.StateMsachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    private readonly ILogger<OrderStateMachine> _logger;

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger;

        Event(() => OrderCreatedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => OrderValidatedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => OrderValidatedFailedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => PaymentProcessedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => PaymentProcessedFailedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => OrderCompletedEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        Event(() => OrderCancelledEvent, x =>
            x.CorrelateById(m => m.Message.OrderId));

        InstanceState(x => x.CurrentState);

        // Khởi tạo
        Initially(
            When(OrderCreatedEvent) // order-created
                .Then(context =>
                {
                    context.Instance.ModifiedOnUtc = DateTime.UtcNow;

                    AuditLogs(nameof(OrderCreatedEvent), context.Instance.CorrelationId);
                })
                .TransitionTo(Created)
        );

        // Me: sau khi tạo Order, thì bây giờ trạng thái State là Order
        // Mình sẽ đứng chờ xem OrderValidated thành công hay thất bại từ 2 event <=> 2 queue
        // Nếu thành công -> state process của payment
        // Nếu thất bại -> state cancel
        During(Created,
            Ignore(OrderCreatedEvent),
            When(OrderValidatedEvent) // OrderValidatedConsumer
                .Then(context =>
                {
                    AuditLogs(nameof(OrderValidatedEvent), context.Instance.CorrelationId);
                })
                .TransitionTo(Processed),
            When(OrderValidatedFailedEvent) // OrderValidatedFailedConsumer
                .Then(context =>
                {
                    AuditLogs(nameof(OrderValidatedFailedEvent), context.Instance.CorrelationId);
                })
                .TransitionTo(Canceled)
        );

        During(Processed,
            Ignore(OrderCreatedEvent),
            Ignore(OrderValidatedEvent),
            When(PaymentProcessedEvent)
                .Then(context =>
                {
                    AuditLogs(nameof(PaymentProcessedEvent), context.Instance.CorrelationId);
                })
                .TransitionTo(Completed),
            When(PaymentProcessedFailedEvent)
                .Then(context =>
                {
                    AuditLogs(nameof(PaymentProcessedFailedEvent), context.Instance.CorrelationId);
                })
                .TransitionTo(Canceled)
         );

        During(Completed,
            Ignore(PaymentProcessedEvent),
            When(OrderCompletedEvent)
                .Then(context =>
                {
                    AuditLogs(nameof(OrderCompletedEvent), context.Instance.CorrelationId);
                })
                .Finalize());

        During(Canceled,
            Ignore(OrderValidatedFailedEvent),
            Ignore(PaymentProcessedFailedEvent),
            When(OrderCancelledEvent)
                .Then(context =>
                {
                    AuditLogs(nameof(OrderCancelledEvent), context.Instance.CorrelationId);
                })
                .Finalize());

        SetCompletedWhenFinalized();
    }

    #region state

    public State Created { get; private set; } = null!;
    public State Processed { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Canceled { get; private set; } = null!;

    #endregion state

    #region event

    public Event<DomainEvent.OrderCreated> OrderCreatedEvent { get; private set; } = null!;
    public Event<DomainEvent.StockReversed> OrderValidatedEvent { get; private set; } = null!; // Reverse stock
    public Event<DomainEvent.StockReversedFailed> OrderValidatedFailedEvent { get; private set; } = null!; // Reverse stock failed
    public Event<DomainEvent.PaymentProcessed> PaymentProcessedEvent { get; private set; } = null!;
    public Event<DomainEvent.PaymentProcessedFailed> PaymentProcessedFailedEvent { get; private set; } = null!;
    public Event<DomainEvent.OrderCompleted> OrderCompletedEvent { get; private set; } = null!;
    public Event<DomainEvent.OrderCancelled> OrderCancelledEvent { get; private set; } = null!;

    #endregion event

    private void AuditLogs(string eventName, Guid correlationId, string description = "")
    {
        _logger.LogInformation("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");

        _logger.Log(LogLevel.Information,
           "[{Event}]-[{AuditedAt}] Actor is [{Actor}] with Order is [{CorrelateId}] and Status is [{Status}]",
           eventName,
           DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
           $"[{nameof(OrderStateMachine)}]",
           correlationId.ToString(),
           eventName);
    }
}