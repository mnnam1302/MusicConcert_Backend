namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Response
{
    public record AuthenticateResponse(string AccessToken, string RefreshToken, DateTime? RefreshTokenExpiryTime);
}