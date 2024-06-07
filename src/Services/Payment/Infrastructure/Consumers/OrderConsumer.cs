using Contracts.Services.V1.Order;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers;

public static class OrderConsumer
{
    public class  OrderValidatedConsumer : Consumer<DomainEvent.OrderValidated>
    {
        public OrderValidatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}