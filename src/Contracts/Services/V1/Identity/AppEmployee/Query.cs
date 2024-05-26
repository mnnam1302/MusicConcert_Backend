using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Query
{
    public record EmployeeLoginQuery(
        string Email, 
        string Password) : IQuery<Response.AuthenticateResponse>;

    public record EmployeeRefreshTokenQuery(
        string Email, 
        string AccessToken, 
        string RefreshToken) : IQuery<Response.AuthenticateResponse>;
}