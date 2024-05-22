using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity;

public static class DomainEvent
{
    public record OrganizationCreated(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id,
        string Name) : IDomainEvent, ICommand;

    public record OrganizationUpdated(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id,
        string Name) : IDomainEvent;

    public record OrganizationDeleted(Guid EventId, DateTimeOffset TimeStamp, Guid Id) : IDomainEvent;
}