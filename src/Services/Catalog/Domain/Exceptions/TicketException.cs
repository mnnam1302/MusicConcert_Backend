namespace Domain.Exceptions;

public static class TicketException
{
    public class TicketNotFoundException : NotFoundException
    {
        public TicketNotFoundException(Guid ticketId)
            : base($"Ticket with Id {ticketId} was not found.")
        {
        }
    }
}