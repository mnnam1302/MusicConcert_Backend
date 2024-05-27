using Carter;
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

        group1.MapGet("employee/sign-in", SignInEmployeesV1);
        group1.MapGet("employee/sign-out", SignOutEmployeesV1);

        group1.MapGet("customer/sign-in", SignInCustomersV1);
        group1.MapGet("customer/sign-out", SignOutCustomersV1);
    }

    private static async Task<IResult> SignInEmployeesV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.AppEmployee.Query.LoginEmployeeQuery request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> SignOutEmployeesV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.AppEmployee.Command.LogoutEmployeeCommand request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> SignInCustomersV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.Customer.Query.LoginCustomerQuery request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> SignOutCustomersV1(ISender sender, [FromBody] Contracts.Services.V1.Identity.Customer.Command.LogoutCustomerCommand request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}