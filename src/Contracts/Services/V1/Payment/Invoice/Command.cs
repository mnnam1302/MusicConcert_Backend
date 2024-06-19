using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Payment.Invoice;

public static class Command
{
    public record PaymentInvoiceCommand(Guid? InvoiceId, Guid OrderId, Guid CustomerId, string TransactionCode) : ICommand;

    public record CancelInvoiceCommand(Guid InvoiceId, Guid OrderId, Guid CustomerId) : ICommand;
}