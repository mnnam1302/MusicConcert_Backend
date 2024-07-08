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

    //public record OrderDetailsResponse(Guid Id, Guid TicketName, int Quantity, decimal UnitPrice);
    public record OrderDetailsResponse
    {
        public OrderDetailsResponse()
        {
        }

        public Guid Id { get; init; }
        public string TicketName { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal TotalPrice { get; init;}
    }
}