using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;

namespace Contracts.Services.V1.Catalog.Event;

public static class Query
{
    public record GetEventByIdQuery(Guid Id) : IQuery<Response.EventDetailsReponse>;

    public record GetEventsQuery(
        string? SearchTerm, 
        string? SortColumn, 
        string? SortOrder, 
        DateTime? StartedDate, 
        int PageIndex, 
        int PageSize) : IQuery<PagedResult<Response.EventResponse>>;


    public record GetEventsByOrganizationId(
        Guid OrganizationId,
        string? City,
        Guid? eventId,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<Response.EventResponse>>;
}