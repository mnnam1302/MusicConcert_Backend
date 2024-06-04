namespace Domain.Exceptions;

public static class TicketException
{
    public class TicketNotFoundException : NotFoundException
    {
        public TicketNotFoundException(Guid ticketId)
            : base($"The ticket with Id {ticketId} was not found.")
        {
        }
    }

    public class TicketQuantityNotEnoughException : Exception
    {
        public TicketQuantityNotEnoughException(Guid ticketId)
            //: base($"Not enough ticket in stock for ticket with Id {ticketId}.")
            : base($"The ticket with Id {ticketId} is not enough.")
        {
        }
    }
}