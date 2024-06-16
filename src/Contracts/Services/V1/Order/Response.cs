namespace Contracts.Services.V1.Order;

public static class Response
{
    public record OrderCanceledResponse(Guid OrderId, string? CanceledReason);
}