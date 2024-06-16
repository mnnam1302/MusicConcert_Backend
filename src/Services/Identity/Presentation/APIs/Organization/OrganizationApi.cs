using Carter;
using Contracts.Services.V1.Identity.Organization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Organization;

public class OrganizationApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/organizations";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Organization")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateOrganizationsV1);
        group1.MapDelete("{organizationId}", DeleteOrganizationsV1);
        //group1.MapPut("organizationId", () => "");

        // QUERY //
        group1.MapGet("", GetOrganizationsV1);
        group1.MapGet("{organizationId}", GetOrganizationsByIdV1);
    }

    private static async Task<IResult> GetOrganizationsByIdV1(ISender sender, Guid organizationId)
    {
        var query = new Query.GetOrganizaitionByIdQuery(organizationId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetOrganizationsV1(
        ISender sender,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetOrganizationsQuery(pageIndex, pageSize);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateOrganizationsV1(ISender sender, [FromBody] Command.CreateOrganizationCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteOrganizationsV1(ISender sender, Guid organizationId)
    {
        var command = new Command.DeleteOrganizationCommand(organizationId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}