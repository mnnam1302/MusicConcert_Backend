namespace Contracts.Services.V1.Payment.Invoice;

public class Response
{
    public record InvoiceResponse(
        Guid Id,
        decimal SubTotal,
        decimal Discount,
        decimal Tax,
        decimal TotalPrice,
        string? TransactionCode,
        DateTimeOffset? PaymentedOnUtc,
        string Status,
        Guid OrderInfoId);
}