using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class InvoiceStatus : SmartEnum<InvoiceStatus>
{
    public InvoiceStatus(string name, int value)
        : base(name, value)
    {
    }

    public static readonly InvoiceStatus Pending = new(nameof(Pending), 1);
    public static readonly InvoiceStatus Paid = new(nameof(Paid), 2);
    public static readonly InvoiceStatus Cancelled = new(nameof(Cancelled), 3);

    public static InvoiceStatus FromName(string name) 
        => FromName(name, false);
    public static InvoiceStatus FromValue(int value)
        => FromValue(value);

    public static implicit operator int(InvoiceStatus status)
        => status.Value;

    public static implicit operator string(InvoiceStatus status)
        => status.Name;
}