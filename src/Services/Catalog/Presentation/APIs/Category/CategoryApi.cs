using Carter;
using Contracts.Services.V1.Catalog.Category;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.Category;

public class CategoryApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/categories";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Category")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateCategoriesV1);
        group1.MapGet("", GetCategoriesV1);
        group1.MapGet("{categoryId}", GetCategoriesByIdV1);
        group1.MapPut("{categoryId}", UpdateCategoriesV1);
        group1.MapDelete("{categoryId}", DeleteCategoriesV1);
    }

    private static async Task<IResult> GetCategoriesByIdV1(ISender sender, Guid categoryId)
    {
        var query = new Query.GetCategoryByIdQuery(categoryId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCategoriesV1(
        ISender sender,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetCategoriesQuery(pageIndex, pageSize);
        var result = await sender.Send(query);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateCategoriesV1(ISender sender, [FromBody] Command.CreateCategoryCommand request)
    {
        var result = await sender.Send(request);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateCategoriesV1(ISender sender, Guid categoryId, [FromBody] Command.UpdateCategoryCommand request)
    {
        var command = request with { Id = categoryId };
        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteCategoriesV1(ISender sender, Guid categoryId)
    {
        var command = new Command.DeleteCategoryCommand(categoryId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}