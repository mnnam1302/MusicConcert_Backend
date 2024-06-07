using MassTransit;

namespace Infrastructure.StateMachines;

public class OrderState
    : SagaStateMachineInstance,
    ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }

    public string CurrentState { get; set; } = default!;
    public string FaultReason { get; set; } = null!;

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public string TransactionId { get; set; } = null!;
}