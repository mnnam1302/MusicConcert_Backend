using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.Customer;

public class Command
{
    public record CreateCustomerCommand(
        string FirstName,
        string LastName, 
        string PhoneNumber,
        DateTime? DateOfBirth,
        string? Address,
        string Email,
        string Password,
        string PasswordConfirmation) : ICommand;

    public record ChangePasswordCommand(Guid Id, string NewPassword, string NewPasswordConfirmation) : ICommand;

    public record DeleteCustomerCommand(Guid Id) : ICommand;


    // Identity
    public record LogoutCustomerCommand(string Email) : ICommand;
}