namespace Contracts.Services.V1.Order;

public static class Response
{
    public record CreateOrderResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; }
        public decimal TotalPrice { get; init; }
    }

    public record OrderResponse
    {
        public Guid Id { get; init; }
        public string Status { get; init; }
        public DateTimeOffset CreatedOnUtc { get; init; }
        public List<OrderDetailsResponse> OrderDetails { get; init; }
    }

    public record struct OrderDetailsResponse(Guid Id, Guid OrderId, Guid TicketInfoId, int Quantity, decimal UnitPrice);
}