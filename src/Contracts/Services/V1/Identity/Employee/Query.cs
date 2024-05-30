using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Query
{
    public record LoginEmployeeQuery(
        string Email, 
        string Password) : IQuery<Response.AuthenticatedResponse>;

    public record EmployeeRefreshTokenQuery(
        string Email, 
        string AccessToken, 
        string RefreshToken) : IQuery<Response.AuthenticatedResponse>;

    public record GetEmployeeByIdQuery(Guid Id) : IQuery<Response.EmployeeDetailsResponse>;
}