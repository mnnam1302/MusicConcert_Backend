using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class TicketStatus : SmartEnum<TicketStatus>
{
    public TicketStatus(string name, int value)
        : base(name, value)
    {
    }

    public static readonly TicketStatus Available = new TicketStatus(nameof(Available), 1); // Còn vé
    public static readonly TicketStatus SoldOut = new TicketStatus(nameof(SoldOut), 2); // Hết vé
    public static readonly TicketStatus Discontinued = new TicketStatus(nameof(Discontinued), 3); // Ngưng bán

    public static TicketStatus FromName(string name) 
        => FromName(name, false);
    
    public static TicketStatus FromValue(int value) 
        => FromValue(value);

    public static implicit operator string(TicketStatus status) 
        => status.Name;
    
    public static implicit operator int(TicketStatus status) 
        => status.Value;
}