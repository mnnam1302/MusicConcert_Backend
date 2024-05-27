namespace Domain.Exceptions;

public static class CustomerException
{
    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException(string email) 
            : base($"Customer with Email {email} already exists.")
        {
        }
    }

    public class CustomerFieldException : BadRequestException
    {
        public CustomerFieldException(string fieldName)
            : base($"Customer with field {fieldName} is not correct.")
        {
        }
    }

    public class CustomerNotFoundException : NotFoundException
    {
        public CustomerNotFoundException(Guid customerId)
            : base($"Customer with Id {customerId} was not found.")
        {
        }
    }

    public class CustomerNotFoundByEmailException : NotFoundException
    {
        public CustomerNotFoundByEmailException(string email)
            : base($"Customer with Email {email} was not found.")
        {
        }
    }
}