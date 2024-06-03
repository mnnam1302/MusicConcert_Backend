namespace Domain.Exceptions;

public static class TicketInfoException
{
    public class TicketInfoAlreadyExistsException : Exception
    {
        public TicketInfoAlreadyExistsException(Guid ticketId)
            : base($"Ticket with Id {ticketId} already exists.")
        {
        }
    }

    public class TicketInfoNotFoundException : NotFoundException
    {
        public TicketInfoNotFoundException(Guid id)
            : base($"Ticket with Id {id} was not found.")
        {
        }
    }
}