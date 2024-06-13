using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Ticket;

public class Query
{
    public record GetTicketsByEventId(Guid EventId) : IQuery<Response.TicketsEventResponse>;
} 