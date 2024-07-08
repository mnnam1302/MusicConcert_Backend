using Domain.Abstractions.Aggregates;

namespace Domain.Entities;

public class OrderDetails : AggregateRoot<Guid>
{
    protected OrderDetails() { }

    public OrderDetails(Guid id, Guid orderId, Guid ticketId, decimal unitPrice, int quantity)
    {
        Id = id;
        OrderId = orderId;
        TicketInfoId = ticketId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid OrderId { get; private set; }

    public Guid TicketInfoId { get; private set; }
    public virtual TicketInfo TicketInfo { get; private set; } = null!;

    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
}