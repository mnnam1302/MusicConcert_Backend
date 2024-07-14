namespace Contracts.Core.Exceptions;

public class BadRequestException : DomainException
{
    public BadRequestException(string message)
        : base("Bad Request", message)
    {
        
    }
}