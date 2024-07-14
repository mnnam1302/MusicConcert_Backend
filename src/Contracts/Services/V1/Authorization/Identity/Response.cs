namespace Contracts.Services.V1.Authorization.Identity;

public static class Response
{
    public record AuthenticatedResponse
    {
        public Guid UserId { get; init; }
        public string Email { get; init; }
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
    }
}