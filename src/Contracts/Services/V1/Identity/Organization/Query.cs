using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;

namespace Contracts.Services.V1.Identity.Organization;

public static class Query
{
    public record GetOrganizaitionByIdQuery(Guid Id) : IQuery<Response.OrganizationDetailsResponse>;

    public record GetOrganizationsQuery(int PageIndex, int PageSize) : IQuery<PagedResult<Response.OrganizationResponse>>;
}