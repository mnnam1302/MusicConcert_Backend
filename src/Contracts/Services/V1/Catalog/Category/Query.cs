using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Category;

public static class Query
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Response.CategoryResponse>;

    public record GetCategoriesQuery() : IQuery<List<Response.CategoryResponse>>; 
}