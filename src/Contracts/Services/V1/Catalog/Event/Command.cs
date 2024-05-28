using Contracts.Abstractions.Message;
using Contracts.Enumerations;
using System.Diagnostics;

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
        Guid OrganizationId,
        string EventType,
        string? MeetUrl, 
        string? Adrress,
        string? District,
        string? City,
        string? Country) : ICommand;
}