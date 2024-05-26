using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Query
{
    public record GetEmployeeLoginQuery(string Email, string Password) : IQuery<Response.AuthenticateResponse>;
}