namespace Contracts.Core.Exceptions;

public static class IdentityException
{
    public class Unauthorized : DomainException
    {
        public Unauthorized(string message) : 
            base("Unauthorized", message)
        {
        }
    }

    public class Forbidden : DomainException
    {
        public Forbidden(string message) : 
            base("Forbidden", message)
        {
        }
    }
}