namespace Domain.Exceptions;

public class OrderInfoException
{
    public class OrderInfoAlreadyExistsException : Exception
    {
        public OrderInfoAlreadyExistsException(Guid orderId)
            : base($"Order with Id {orderId} already exists.")
        {
        }
    }

    public class OrderInfoNotFoundException : NotFoundException
    {
        public OrderInfoNotFoundException(Guid orderId)
            : base($"Order with Id {orderId} was not found.")
        {
        }
    }
}