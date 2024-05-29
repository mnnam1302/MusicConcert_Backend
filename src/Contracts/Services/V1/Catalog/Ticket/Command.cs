

using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Ticket;

public static class Command
{
    public record CreateTicket(string Name, decimal UnitPrice, int UnitInStock, Guid EventId) : ICommand;

    public record DeleteTicket(Guid Id) : ICommand;
}