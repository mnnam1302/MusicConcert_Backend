using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;

namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Query
{
    // AUTH //
    public record LoginEmployeeQuery(
        string Email, 
        string Password) : IQuery<Response.AuthenticatedResponse>;

    public record EmployeeRefreshTokenQuery(
        string Email, 
        string AccessToken, 
        string RefreshToken) : IQuery<Response.AuthenticatedResponse>;

    // QUERY //
    public record GetEmployeeByIdQuery(Guid Id) : IQuery<Response.EmployeeDetailsResponse>;

    public record GetEmployeesByOrganizationQuery(Guid? OrganizationId, int PageIndex, int PageSize) : IQuery<PagedResult<Response.EmployeesResponse>>;
}