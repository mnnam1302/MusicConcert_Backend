using MassTransit;

namespace Contracts.Abstractions.Message;

[ExcludeFromTopology]
public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset TimeStamp { get; }
}