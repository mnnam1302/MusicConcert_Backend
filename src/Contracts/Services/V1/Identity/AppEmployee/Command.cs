using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Command
{
    public record CreateEmployeeCommand(
        string FirstName, 
        string LastName, 
        string PhoneNumber, 
        DateTime? DateOfBirth,
        string Email,
        string Password,
        string PasswordConfirmation,
        Guid? OrganizationId) 
        : ICommand;

    public record UpdateEmployeeCommand(
        Guid Id, 
        string FirstName, 
        string LastName, 
        DateTimeOffset? DateTimeOffset, 
        Guid? OrganizationId) : ICommand;

    public record DeleteEmployeeCommand(Guid Id) : ICommand;

    public record LogoutEmployeeCommand(string Email, string AccessToken) : ICommand;
}