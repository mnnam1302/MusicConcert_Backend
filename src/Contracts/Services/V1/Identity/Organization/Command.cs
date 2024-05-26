using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.Organization;

public static class Command
{
    public record CreateOrganizationCommand(
        string Name,
        string Industry,
        string? Description,
        string Phone,
        string HomePage,
        string? LogoUrl,
        string Street,
        string City,
        string State,
        string Country,
        string ZipCode) : ICommand;

    public record UpdateOrganizationCommand(
        Guid Id,
        string Name,
        string? Description,
        string Phone,
        string HomePage,
        string? LogoUrl ) : ICommand;

    public record class DeleteOrganizationCommand(Guid Id) : ICommand;
}