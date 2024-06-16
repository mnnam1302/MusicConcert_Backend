using Carter;
using Contracts.Extensions;
using Contracts.Services.V1.Catalog.Event;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Event;


public class EventApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/events";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Event")
            .MapGroup(BaseUrl).HasApiVersion(1);


        group1.MapPost("", CreateEventsV1);
        group1.MapGet("", GetEventsV1);
        group1.MapGet("{eventId}", GetEventsByIdV1);
        group1.MapGet("{eventId}/tickets", GetTicketsByEventIdV1);
        group1.MapPut("/publish/{eventId}", UpdateEventsV1);
        group1.MapDelete("{eventId}", DeleteEventsV1);
    }

    private static async Task<IResult> GetEventsV1(ISender sender,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        DateTime? startedDate = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetEventsQuery(
            searchTerm,
            sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            startedDate is not null ? startedDate : DateTime.Now, 
            pageIndex,
            pageSize);

        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetEventsByIdV1(ISender sender, Guid eventId)
    {
        var query = new Query.GetEventByIdQuery(eventId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetTicketsByEventIdV1(ISender sender, Guid eventId)
    {
        var query = new Contracts.Services.V1.Catalog.Ticket.Query.GetTicketsByEventId(eventId);

        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateEventsV1(ISender sender, [FromBody] Command.CreateEventCommand request)
    {
        var command = request with { EventType = EventTypeExtension.ConvertStringToEventType(request.EventType) };
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    //private static async Task<IResult> UpdateEventsV1(ISender sender,
    //    HttpContext context,
    //    [AsParameters] Command.UpdateEventCommand request)
    private static async Task<IResult> UpdateEventsV1(ISender sender, Guid eventId, HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var files = form.Files;

        //var command = new Command.UpdateEventCommand(files["LogoImage"], files["LayoutImage"]);
        var command = new Command.UpdateEventCommand
        {
            Id = eventId,
            Name = form[nameof(Command.UpdateEventCommand.Name)],
            Description = form[nameof(Command.UpdateEventCommand.Description)],
            LogoImage = files[nameof(Command.UpdateEventCommand.LogoImage)],
            LayoutImage = files[nameof(Command.UpdateEventCommand.LayoutImage)]
        };

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    public static async Task<IResult> DeleteEventsV1(ISender sender, Guid eventId)
    {
        var command = new Command.DeleteEventCommand(eventId);
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}