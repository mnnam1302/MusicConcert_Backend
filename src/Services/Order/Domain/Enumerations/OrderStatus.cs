using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class OrderStatus : SmartEnum<OrderStatus>
{
    public OrderStatus(string name, int value)
        : base(name, value)
    {
    }

    public static readonly OrderStatus OrderCreated = new OrderStatus(nameof(OrderCreated), 1);
    public static readonly OrderStatus OrderValidated = new OrderStatus(nameof(OrderValidated), 2);
    public static readonly OrderStatus OrderValidatedFailed = new OrderStatus(nameof(OrderValidatedFailed), 3);
    public static readonly OrderStatus PaymentProcessed = new OrderStatus(nameof(PaymentProcessed), 4);
    public static readonly OrderStatus PaymentProcessedFailed = new OrderStatus(nameof(PaymentProcessedFailed), 5);
    public static readonly OrderStatus OrderCompleted = new OrderStatus(nameof(OrderCompleted), 6);
    public static readonly OrderStatus OrderCancelled = new OrderStatus(nameof(OrderCancelled), 7);

    public static OrderStatus FromName(string name) 
        => FromName(name);
    public static OrderStatus FromValue(int value) 
        => FromValue(value);

    public static implicit operator string(OrderStatus status)
        => status.Name;

    public static implicit operator int(OrderStatus status)
        => status.Value;
}