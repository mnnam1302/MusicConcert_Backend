namespace Contracts.Services.V1.Order;

public static class Response
{
    public record OrderResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; }
        public decimal TotalPrice { get; init; }
    }
}