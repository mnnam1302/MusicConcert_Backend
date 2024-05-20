namespace Contracts.Abstractions.Message;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset TimeStamp { get; }
}