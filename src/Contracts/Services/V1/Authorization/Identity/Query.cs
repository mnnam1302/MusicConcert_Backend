using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Authorization.Identity;

public static class Query
{
    public record GetLoginQuery(string Email, string Password) 
        : IQuery<Response.AuthenticatedResponse>;

    public record TokenQuery(string Email, string? AccessToken, string? RefreshToken) 
        : IQuery<Response.AuthenticatedResponse>;
}