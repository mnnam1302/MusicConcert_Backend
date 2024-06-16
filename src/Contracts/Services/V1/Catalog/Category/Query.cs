using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;

namespace Contracts.Services.V1.Catalog.Category;

public static class Query
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Response.CategoryResponse>;

    public record GetCategoriesQuery(int PageIndex, int PageSize) : IQuery<PagedResult<Response.CategoryResponse>>; 
}