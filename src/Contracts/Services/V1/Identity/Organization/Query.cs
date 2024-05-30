using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Identity.Organization;

public static class Query
{
    public record GetOrganizaitionByIdQuery(Guid Id) : IQuery<Response.OrganizationDetailsResponse>;

    public record GetOrganizationsQuery() : IQuery<List<Response.OrganizationResponse>>;
}