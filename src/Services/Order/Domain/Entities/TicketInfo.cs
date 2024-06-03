using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class TicketInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    public TicketInfo(Guid ticketId)
    {
        Id = Guid.NewGuid();
        TicketId = ticketId;
    }

    public Guid TicketId { get; private set; } // Surrogate key

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}