using Carter;
using Contracts.Services.V1.Identity.Customer;
using MassTransit.Caching.Internals;
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
        group1.MapGet("", GetCustomersV1);
        group1.MapGet("{customerId}", GetCustomersByIdV1);
        //group1.MapPut("customerId", () => "");
        group1.MapDelete("{customerId}", DeleteCustomersV1);
    }

    private static async Task<IResult> GetCustomersByIdV1(ISender sender, Guid customerId)
    {
        var query = new Query.GetCustomerByIdQuery(customerId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCustomersV1(ISender sender)
    {
        var query = new Query.GetCustomersQuery();
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateCustomersV1(ISender sender, [FromBody] Command.CreateCustomerCommand request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteCustomersV1(ISender sender, Guid customerId)
    {
        var command  = new Command.DeleteCustomerCommand(customerId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}