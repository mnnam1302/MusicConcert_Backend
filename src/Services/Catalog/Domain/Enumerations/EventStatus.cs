using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class EventStatus : SmartEnum<EventStatus>
{
    public EventStatus(string name, int value)
        : base(name, value)
    {
    }

    public static readonly EventStatus NonPublished = new(nameof(NonPublished), 1);
    public static readonly EventStatus Published = new(nameof(Published), 2);
    public static readonly EventStatus Completed = new(nameof(Completed), 3);
    public static readonly EventStatus Cancelled = new(nameof(Cancelled), 4);

    public static implicit operator EventStatus(string name) 
        => FromName(name);

    public static implicit operator EventStatus(int value)
        => FromValue(value);

    public static implicit operator string(EventStatus eventStatus)
        => eventStatus.Name;

    public static implicit operator int(EventStatus eventStatus)
        => eventStatus.Value;
}