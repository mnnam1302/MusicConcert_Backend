using Carter;
using Contracts.Services.V1.Identity.AppEmployee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Identity;

public class TokenApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/token";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Token")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("/employee/refresh", RefreshTokenV1);
        group1.MapPost("/employee/revoke", () => "");
    }

    private static async Task<IResult> RefreshTokenV1(ISender sender, [FromBody] Query.EmployeeRefreshTokenQuery request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}