using Contracts.Abstractions.Message;
using Microsoft.AspNetCore.Http;

namespace Contracts.Services.V1.Catalog.Event;

/// <summary>
/// Represents a command to create an event.
/// </summary>
public static class Command
{
    public record CreateEventCommand(
        string Name,
        string? Description,
        DateTimeOffset StartedDateOnUtc,
        DateTimeOffset EndedDateOnUtc,
        int Capacity,
        Guid? CategoryId,
        Guid? OrganizationId,
        string EventType,
        string? MeetUrl,
        string? Adrress,
        string? District,
        string? City,
        string? Country) : ICommand;

    public record UpdateEventCommand : ICommand
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }

        public IFormFile? LogoImage { get; init; }
        public IFormFile? LayoutImage { get; init; }
    }

    public record DeleteEventCommand(Guid Id) : ICommand;
}