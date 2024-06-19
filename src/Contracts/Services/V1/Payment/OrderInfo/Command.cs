using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Payment.OrderInfo;

public static class Command
{
    public record PaymentOrderCommand(Guid? OrderId, Guid CustomerId, string TransactionCode) : ICommand;

    public record CancelOrderCommand(Guid? OrderId, Guid CustomerId) : ICommand;
}