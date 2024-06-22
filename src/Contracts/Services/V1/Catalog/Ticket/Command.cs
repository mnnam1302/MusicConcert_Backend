

using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Ticket;

public static class Command
{
    #region V1
    public record CreateTicket(string Name, decimal UnitPrice, int UnitInStock, Guid EventId) : ICommand;
    public record DeleteTicket(Guid Id) : ICommand;

    #endregion V1


    #region V2
    public record CreateTicketCommandV2 : ICommand
    {
        public Guid EventId { get; init; }
        public List<TicketItem> Tickets { get; init; }
    }

    public record TicketItem(string Name, decimal UnitPrice, int UnitInStock);

    #endregion V2
}