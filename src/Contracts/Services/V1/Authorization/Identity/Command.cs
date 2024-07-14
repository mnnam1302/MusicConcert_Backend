using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Authorization.Identity;

public static class Command
{
    public record RevokeTokenCommand(string Email, string AccessToken) : ICommand;

    public record LogoutCommand(string Email, string AccessToken) : ICommand;
}