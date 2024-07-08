﻿using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Ticket;

public static class DomainEvent
{
    public record TicketCreated(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id,
        string Name) : IDomainEvent, ICommand;

    public record TicketDeleted(
        Guid EventId,
        DateTimeOffset TimeStamp,
        Guid Id) : IDomainEvent, ICommand;
}