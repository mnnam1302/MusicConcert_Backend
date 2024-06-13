namespace Contracts.Services.V1.Catalog.Ticket;

public static class Response
{
    public record TicketsEventResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<TicketResponse> Tickets { get; init; }
    }

    public record TicketResponse(Guid Id, string Name, decimal UnitPrice, int UnitInStock, string Status);
}