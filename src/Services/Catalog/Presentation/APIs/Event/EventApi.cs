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
        group1.MapPut("{eventId}", UpdateEventsV1);
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
            IsPublished = string.IsNullOrEmpty(form[nameof(Command.UpdateEventCommand.IsPublished)]) 
                    ? false : true,
            LogoImage = files[nameof(Command.UpdateEventCommand.LogoImage)],
            LayoutImage = files[nameof(Command.UpdateEventCommand.LayoutImage)]
        };

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}