using Contracts.Services.V1.Order;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

public class Order : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    public Order()
    {
    }

    private Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerInfoId = customerId;
    }

    public static Order Create(Guid customerId, List<Command.OrderDetail> details)
    {
        // Step 01: check business rule
        if (details.Count == 0)
            throw new OrderException.OrderFieldException(nameof(OrderDetails));

        if (!details.Select(x => x.Quantity > 0 && x.Quantity <= 5).Any())
            throw new OrderDetailsException.OrderDetailsWithQuantityException();

        // Step 02: Create order
        var order = new Order(customerId);

        foreach (var detail in details)
        {
            order.InsertOrderDetail(Guid.NewGuid(), order.Id, detail.TicketId, detail.UnitPrice, detail.Quantity);
        }

        // Step 03: Raise event
        order.RaiseDomainEvent(new DomainEvent.OrderCreated
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTime.UtcNow,
            OrderId = order.Id,
            CustomerId = order.CustomerInfoId,
            TotalAmount = order.OrderDetails.Sum(x => x.UnitPrice * x.Quantity),

            Details = order.OrderDetails.Select(x => new DomainEvent.OrderDetail(
                x.Id,
                order.Id,
                x.TicketInfoId,
                x.UnitPrice,
                x.Quantity)).ToList()
        });

        return order;
    }

    public Order InsertOrderDetail(Guid id, Guid orderId, Guid ticketId, decimal unitPrice, int quantity)
    {
        //float discountValue = discount.HasValue ? discount.Value : 0.1f;

        OrderDetails.Add(new OrderDetails(id, orderId, ticketId, unitPrice, quantity));
        return this;
    }

    public Order AssignValidatedSuccess(OrderStatus status, decimal totalAmount)
    {
        RaiseDomainEvent(new DomainEvent.OrderValidated
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTime.UtcNow,
            OrderId = Id,
            CustomerId = CustomerInfoId,
            TotalAmount = totalAmount,
        });

        Status = status;
        return this;
    }

    public Order AssignValidatedFailed(OrderStatus status, string reason)
    {
        Status = status;

        RaiseDomainEvent(new DomainEvent.OrderCancelled
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTime.UtcNow,
            OrderId = Id,
            CustomerId = CustomerInfoId,
            Reason = reason
        });

        return this;
    }

    public Order AssignPaymentProcessed(OrderStatus status, string transactionCode)
    {
        Status = status;

        RaiseDomainEvent(new DomainEvent.OrderCompleted
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTime.UtcNow,
            OrderId = Id,
            TransactionCode = transactionCode
        });

        return this;
    }

    public Order AssignPaymentProcessedFailed(OrderStatus status, string reason)
    {
        Status = status;

        RaiseDomainEvent(new DomainEvent.OrderCancelled
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTime.UtcNow,
            OrderId = Id,
            CustomerId = CustomerInfoId,
            Reason = reason
        });

        return this;
    }

    public Guid CustomerInfoId { get; set; }
    public virtual  CustomerInfo CustomerInfo { get; set; } // must belong one CustomerInfo

    public string Status { get; set; } = OrderStatus.OrderCreated;

    public virtual List<OrderDetails> OrderDetails { get; private set; } = new();

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}