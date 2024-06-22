using Carter;
using Contracts.Services.V1.Catalog.Ticket;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Ticket;

public class TicketApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/tickets";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Ticket")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateTicketV1);
        group1.MapDelete("{ticketId}", DeleteTicketsV1);

        var group2 = app.NewVersionedApi("Ticket")
            .MapGroup(BaseUrl).HasApiVersion(2);

        group2.MapPost("", CreateTicketV2);
    }

    #region V1
    private static async Task<IResult> CreateTicketV1(ISender sender, [FromBody] Command.CreateTicket command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteTicketsV1(ISender sender, Guid ticketId)
    {
        var command = new Command.DeleteTicket(ticketId);
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    #endregion V1

    #region V2

    private static async Task<IResult> CreateTicketV2(ISender sender, [FromBody] Command.CreateTicketCommandV2 command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    #endregion V2
}