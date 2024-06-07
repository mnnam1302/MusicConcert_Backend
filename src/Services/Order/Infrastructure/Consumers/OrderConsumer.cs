using Contracts.Services.V1.Order;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers;

public static class OrderConsumer
{
    public class StockReversedConsumer : Consumer<DomainEvent.StockReversed>
    {
        public StockReversedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    public class StockReversedFailedConsumer : Consumer<DomainEvent.StockReversedFailed>
    {
        public StockReversedFailedConsumer(ISender sender)
            : base(sender)
        {
        }
    }


}