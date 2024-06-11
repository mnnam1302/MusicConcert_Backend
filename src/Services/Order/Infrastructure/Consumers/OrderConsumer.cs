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

    public class PaymentProcessedConsumer : Consumer<DomainEvent.PaymentProcessed>
    {
        public PaymentProcessedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    public class PaymentProcessedFailedConsumer : Consumer<DomainEvent.PaymentProcessedFailed>
    {
        public PaymentProcessedFailedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}