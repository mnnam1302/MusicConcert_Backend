using Carter;
using Contracts.Services.V1.Payment.OrderInfo;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractsions;

namespace Presentation.APIs.OrderInfo;

public class OrderInfoApi : ApiEndpoint, ICarterModule
{
    private readonly string BaseUrl = "/api/v{version:apiVersion}/orders";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("OrderInfo")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPut("{orderId}/payment", PaymentOrderV1);
        group1.MapPut("{orderId}/cancel", CancelOrderV1);
    }

    private static async Task<IResult> PaymentOrderV1(ISender sender, Guid orderId, [FromBody] Command.PaymentOrderCommand request)
    {
        var command = new Command.PaymentOrderCommand(orderId, request.CustomerId, request.TransactionCode);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CancelOrderV1(ISender sender, Guid orderId, [FromBody] Command.CancelOrderCommand request)
    {
        var command = new Command.CancelOrderCommand(orderId, request.CustomerId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}