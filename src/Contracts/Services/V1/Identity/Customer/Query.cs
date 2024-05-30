using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.Customer;

public static class Query
{
    public record LoginCustomerQuery(string Email, string Password) : IQuery<Response.AuthenticatedResponse>;

    public record CustomerRefreshTokenQuery(
        string Email,
        string AccessToken,
        string RefreshToken) : IQuery<Response.AuthenticatedResponse>;

    public record GetCustomerByIdQuery(Guid Id) : IQuery<Response.CustomerDetailsResponse>;

    public record GetCustomersQuery() : IQuery<List<Response.CustomerResponse>>;
}