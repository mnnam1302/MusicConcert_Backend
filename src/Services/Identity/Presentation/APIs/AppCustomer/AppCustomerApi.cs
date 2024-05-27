using Carter;
using Contracts.Services.V1.Identity.Customer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.AppCustomer;

public class AppCustomerApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/customers";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Customer")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateCustomersV1);
        group1.MapGet("customerId", () => "");
        group1.MapGet("", () => "");
        group1.MapPut("customerId", () => "");
        group1.MapDelete("customer", () => "");
    }

    private static async Task<IResult> CreateCustomersV1(ISender sender, [FromBody] Command.CreateCustomerCommand request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}