namespace Contracts.Services.V1.Catalog.Event;

public static class Response
{
    public record CreateEventResponse(
        Guid Id,
        string Name,
        string? Description);

    public record EventResponse(
        Guid Id,
        string Name,
        string? Description,
        string? LogoImage,
        string Status,
        DateTimeOffset StartedOnUtc,
        DateTimeOffset EndedOnUtc,
        DateTimeOffset? PublishedOnUtc,
        string EventType,
        int Capacity,
        bool IsDeleted);

    public record EventDetailsReponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string CategoryName { get; init; }
        public string EventType { get; init; }
        public string OrganizationName { get; init; }
        public string? LogoImage { get; init; }
        public string? LayoutImage { get; init; }
        public string Status { get; init; }
        public DateTimeOffset StartedOnUtc { get; init; }
        public DateTimeOffset EndedOnUtc { get; init; }
        public DateTimeOffset? PublishedOnUtc { get; init; }
        public int Capacity { get; init; }
        public string? MeetUrl { get; init; }
        public string? Addrees { get; init; }
        public string? District { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }

        public List<Contracts.Services.V1.Catalog.Ticket.Response.TicketResponse> Tickets { get; init; }
    }
}