using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Order;

public static class DomainEvent
{
    public record OrderCreated : IDomainEvent
    {
        public Guid EventId { get; init; }
        public DateTimeOffset TimeStamp { get; init; }

        public Guid Id { get; init; }
        public Guid CustomerId { get; init; }
        public decimal TotalAmount { get; init; }
        public List<OrderDetail> Details { get; set; } = new();
    }

    // ============= OrderDetail =============
    // Chưa làm Discount
    //public readonly record struct OrderDetail(
    //    Guid Id,
    //    Guid OrderId,
    //    Guid TicketId,
    //    decimal UnitPrice,
    //    int Quantity,
    //    float Discount);
    public readonly record struct OrderDetail(
        Guid Id,
        Guid OrderId,
        Guid TicketId,
        decimal UnitPrice,
        int Quantity);
}