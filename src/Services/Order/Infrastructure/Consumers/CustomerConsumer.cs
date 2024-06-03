using Contracts.Services.V1.Identity.Customer;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers;

public class CustomerConsumer
{
    public class CustomerCreatedConsumer : Consumer<DomainEvent.CustomerCreated>
    {
        public CustomerCreatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    public class CustomerUpdatedConsumer : Consumer<DomainEvent.CustomerUpdated>
    {
        public CustomerUpdatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    public class CustomerDeletedConsumer : Consumer<DomainEvent.CustomerDeleted>
    {
        public CustomerDeletedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}