namespace Domain.Exceptions;

public static class CustomerInfoException
{
    public class CustomerInfoAlreadyExistsException : Exception
    {
        public CustomerInfoAlreadyExistsException(Guid customerId)
            : base($"Customer with Id {customerId} already exists.")
        {
        }
    }

    public class CustomerInfoNotFoundException : NotFoundException
    {
        public CustomerInfoNotFoundException(Guid id)
            : base($"Customer with Id {id} was not found.")
        {
        }
    }
}