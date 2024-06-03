using Contracts.Services.V1.Catalog.Ticket;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers;

public static class TicketConsumer
{
    public class TicketCreatedConsumer : Consumer<DomainEvent.TicketCreated>
    {
        public TicketCreatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    public class TicketDeletedConsumer : Consumer<DomainEvent.TicketDeleted>
    {
        public TicketDeletedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}