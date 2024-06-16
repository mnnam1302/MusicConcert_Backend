using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

public class Ticket : AggregateRoot<Guid>, ISoftDeleted, IAuditable
{
    protected Ticket()
    { }

    private Ticket(Guid id, string name, decimal unitPrice, int unitInStock, Guid eventId)
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        UnitInStock = unitInStock;
        EventId = eventId;
    }

    public static Ticket Create(string name, decimal unitPrice, int unitInStock, Guid eventId)
    {
        var ticket = new Ticket(Guid.NewGuid(), name, unitPrice, unitInStock, eventId);

        ticket.RaiseDomainEvent(new Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketCreated(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            ticket.Id));

        return ticket;
    }

    public void Delete()
    {
        Status = TicketStatus.Discontinued;

        RaiseDomainEvent(new Contracts.Services.V1.Catalog.Ticket.DomainEvent.TicketDeleted(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            Id));
    }

    public Ticket AssignQuantity(int quantity)
    {
        var newUnitInStock = UnitInStock - quantity;

        if (newUnitInStock < 0)
        {
            throw new TicketException.TicketQuantityNotEnoughException(Id);
        }
        else if (newUnitInStock == 0)
        {
            UnitInStock = newUnitInStock;
            Status = TicketStatus.SoldOut;
        }
        else
        {
            UnitInStock = newUnitInStock;
        }

        return this;
    }

    public Ticket ReverseQuantity(int reverseNumber)
    {
        if (UnitInStock == 0)
            Status = TicketStatus.Available;

        UnitInStock += reverseNumber;
        return this;
    }

    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int UnitInStock { get; private set; }

    public string Status { get; private set; } = TicketStatus.Available;

    public Guid EventId { get; private set; }
    //public virtual Event Event { get; private set; } = null!;

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}