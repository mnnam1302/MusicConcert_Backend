using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class Status : SmartEnum<Status>
{
    public Status(string name, int value)
        : base(name, value)
    {
    }

    public static readonly Status Pending = new(nameof(Pending), 1);
    public static readonly Status Active = new(nameof(Active), 2);
    public static readonly Status InActive = new(nameof(InActive), 3);
    public static readonly Status Deny = new(nameof(Deny), 4);

    public static implicit operator Status(string name) 
        => FromName(name);

    public static implicit operator Status(int value)
        => FromValue(value);

    public static implicit operator string(Status status)
        => status.Name;

    public static implicit operator int(Status status)
        => status.Value;
}