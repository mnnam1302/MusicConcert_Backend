﻿using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Domain.Entities;

public class Ticket : AggregateRoot<Guid>, ISoftDeleted, IAuditable
{
    protected Ticket() { }

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

        ticket.RaiseDomainEvent(new DomainEvent.TicketCreated(
            Guid.NewGuid(), 
            DateTimeOffset.UtcNow, 
            ticket.Id));

        return ticket;
    }

    public void Delete()
    {
        Status = TicketStatus.Discontinued;

        RaiseDomainEvent(new DomainEvent.TicketDeleted(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            Id));
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