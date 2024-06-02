using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.Customer;

public class DomainEvent
{
    public record CustomerCreated(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id,
        string FullName,
        string Email,
        string PhoneNumber) : IDomainEvent, ICommand;

    public record CustomerUpdated(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id,
        string FullName,
        string Email,
        string PhoneNumber) : IDomainEvent, ICommand;

    public record CustomerDeleted(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id) : IDomainEvent, ICommand;
}