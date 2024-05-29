using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Event;

public static class Query
{
    public record GetEventByIdQuery(Guid Id) : IQuery<Response.EventDetailsReponse>;

    public record GetEventsQuery() : IQuery<List<Response.EventResponse>>;
}