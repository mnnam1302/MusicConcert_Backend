namespace Domain.Exceptions;

public static class IdentityException
{
    public class AuthenticationException : BadRequestException
    {
        public AuthenticationException()
            : base("The email or password is not correct.")
        { 
        }
    }
}