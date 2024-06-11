using Carter;
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

        group1.MapPost("/employee/refresh", EmployeeRefreshTokenV1);
        group1.MapPost("/customer/refresh", CustomerRefreshTokenV1);
    }

    private static async Task<IResult> EmployeeRefreshTokenV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.AppEmployee.Query.EmployeeRefreshTokenQuery request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CustomerRefreshTokenV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.Customer.Query.CustomerRefreshTokenQuery request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}