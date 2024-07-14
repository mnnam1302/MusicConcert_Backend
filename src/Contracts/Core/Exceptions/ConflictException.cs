namespace Contracts.Core.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string message) : 
        base("Conflict", message)
    {
    }
}