using Carter;
using Contracts.Services.V1.Identity.AppEmployee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Identity;

public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auth";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Authentication")
            .MapGroup(BaseUrl).HasApiVersion(1);


        group1.MapGet("employee/sign-in", AuthenticationEmployeesV1);
        group1.MapGet("customer/sign-in", () => "");
    }

    private static async Task<IResult> AuthenticationEmployeesV1(ISender sender, [FromBody] Query.GetEmployeeLoginQuery query)
    {
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}