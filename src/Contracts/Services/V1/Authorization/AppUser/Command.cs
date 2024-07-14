using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Authorization.AppUser;

public static class Command
{
    public record RegisterUserCommand(
        string FirstName, 
        string LastName, 
        DateTime? DateOfBirth, 
        string PhoneNumber, 
        string Email, 
        string Password, 
        string PasswordConfirm) : ICommand;
}