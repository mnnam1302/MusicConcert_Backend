using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Order;

public class Query
{
    public record GetOrderByIdQuery(Guid OrderId) : IQuery<Response.OrderResponse>;
}