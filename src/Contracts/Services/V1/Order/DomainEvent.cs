using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Order;

public static class DomainEvent
{
    public record OrderCreated : IDomainEvent, ICommand
    {
        public Guid EventId { get; init; }
        public DateTimeOffset TimeStamp { get; init; }

        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public decimal TotalAmount { get; init; }
        public List<OrderDetail> Details { get; set; } = new();
    }

    public readonly record struct OrderDetail(
        Guid Id,
        Guid OrderId,
        Guid TicketId,
        decimal UnitPrice,
        int Quantity);

    public record StockReversed : IDomainEvent, ICommand
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
    }

    public record StockReversedFailed : IDomainEvent, ICommand
    {
        public Guid EventId { get; init; }
        public DateTimeOffset TimeStamp { get; init; }

        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public string Reason { get; init; }
    }

    public record OrderValidated : IDomainEvent, ICommand
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
    }


    public record PaymentProcessed : IDomainEvent
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string TransactionId { get; set; }
    }

    public record PaymentProcessedFailed : IDomainEvent
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string Reason { get; set; }
        public decimal TotalMoney { get; set; }
        public string TransactionId { get; set; }
    }

    public record OrderCompleted : IDomainEvent
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
        public string TransactionId { get; set; }
    }

    public record OrderCancelled : IDomainEvent
    {
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string Reason { get; set; }
    }
}