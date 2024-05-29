namespace Contracts.Services.V1.Catalog.Event;

public static class Response
{
    public record EventResponse(
        Guid Id,
        string Name,
        string? Description,
        string? LogoImage,
        DateTimeOffset StartedOnUtc,
        DateTimeOffset EndedOnUtc,
        string EventType,
        int Capacity);

    public record EventDetailsReponse(
        Guid Id,
        string Name,
        string? Description,
        string CategoryName,
        string EventType,
        string OrganizationName,
        string? LogoImage,
        string? LayoutImage,
        DateTimeOffset StartedDateOnUtc,
        DateTimeOffset EndedDateOnUtc,
        int Capacity,
        string? MeetUrl,
        string? Addrees,
        string? District,
        string? City,
        string? Country
        );
}