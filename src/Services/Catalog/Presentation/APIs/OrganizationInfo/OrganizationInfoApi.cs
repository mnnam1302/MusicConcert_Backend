using Carter;
using Contracts.Services.V1.Catalog.Event;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;
using static Contracts.Services.V1.Catalog.Ticket.Query;

namespace Presentation.APIs.OrganizationInfo;

public class OrganizationInfoApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/organizations";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("OrganizationInfo")
            .MapGroup(BaseUrl).HasApiVersion(1);

        // QUERY - Customer //
        group1.MapGet("{organizationId}/events", GetEventsByOrganizaitonIdV1);

        group1.MapGet("{organizationId}/events_filter", GetEventsBasedOnStatusByOrganizationIdV1);
    }

    private static async Task<IResult> GetEventsByOrganizaitonIdV1(ISender sender, [FromRoute] Guid organizationId,
        string? city = null,
        Guid? eventId = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetEventsByOrganizationId(organizationId, city, eventId, pageIndex, pageSize);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetEventsBasedOnStatusByOrganizationIdV1(ISender sender, [FromRoute] Guid organizationId,
        string? status = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetEventsBasedOnStatusByOrganizationId(organizationId, status, pageIndex, pageSize);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}