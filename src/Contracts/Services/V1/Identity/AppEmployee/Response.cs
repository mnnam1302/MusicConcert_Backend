namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Response
{
    public record AuthenticateResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
    }
}