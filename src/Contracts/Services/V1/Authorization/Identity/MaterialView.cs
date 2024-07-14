using MassTransit;

namespace Contracts.Services.V1.Authorization.Identity;

public static class MaterialView
{
    public record ApiKey
    {
        public Guid Id { get; init; }
        public string Key { get; init; }
        public List<string> Permissions { get; init; }
    }

    public record AuthenticatedResponse
    {
        public Guid UserId { get; init; }
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
        public string? PublicKey { get; init; }
    }
}