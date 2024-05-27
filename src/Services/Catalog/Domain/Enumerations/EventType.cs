using Ardalis.SmartEnum;
using MassTransit.SagaStateMachine;

namespace Domain.Enumerations;

public class EventType : SmartEnum<EventType>
{
    public EventType(string name, int value)
        : base(name, value)
    {
    }

    public static readonly EventType Online = new(nameof(Online), 1);
    public static readonly EventType Offline = new(nameof(Offline), 2);
    public static readonly EventType Hybrid = new(nameof(Hybrid), 3);

    public static implicit operator EventType(string name) 
        => FromName(name);

    public static implicit operator EventType(int value)
        => FromValue(value);

    public static implicit operator string(EventType eventType)
        => eventType.Name;

    public static implicit operator int(EventType eventType)
        => eventType.Value;
}