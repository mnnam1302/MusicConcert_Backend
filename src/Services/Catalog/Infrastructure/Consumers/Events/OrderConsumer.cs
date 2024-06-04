using Contracts.Services.V1.Order;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers.Events;

public static class OrderConsumer
{
    public class OrderCreatedConsumer : Consumer<DomainEvent.OrderCreated>
    {
        public OrderCreatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}