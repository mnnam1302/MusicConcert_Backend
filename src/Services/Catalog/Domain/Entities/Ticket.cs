using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class Ticket : AggregateRoot<Guid>, ISoftDeleted, IAuditable
{
    protected Ticket()
    {
    }

    private Ticket(Guid id, string name, decimal unitPrice, int unitInStock, Guid eventId)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        UnitInStock = unitInStock;
        EventId = eventId;
    }

    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int UnitInStock { get; private set; }

    public Guid EventId { get; private set; }
    public virtual Event Event { get; private set; } = null!;

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}