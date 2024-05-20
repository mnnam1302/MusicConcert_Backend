using Carter;
using Contracts.Services.V1.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Organization;

public class OrganizationApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/organization";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Organization")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateOrganizationV1);
        //group1.MapGet("", () => "");
        //group1.MapGet("organizationId", () => "");
        //group1.MapPut("organizationId", () => "");
        //group1.MapDelete("organizationId", () => "");
    }

    public static async Task<IResult> CreateOrganizationV1(ISender sender, [FromBody] Command.CreateOrganizationCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}