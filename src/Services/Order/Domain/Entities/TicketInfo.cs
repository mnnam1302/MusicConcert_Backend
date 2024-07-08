using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class TicketInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    public TicketInfo()
    {
    }

    public TicketInfo(Guid ticketId, string name)
    {
        Id = ticketId;
        Name = name;
    }

    public string Name { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}