using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class TicketInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    public TicketInfo()
    {
    }

    public TicketInfo(Guid ticketId)
    {
        Id = ticketId;
        //TicketId = ticketId;
    }

    //public Guid TicketId { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}