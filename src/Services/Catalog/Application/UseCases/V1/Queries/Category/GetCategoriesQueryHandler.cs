using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Category;

public class GetCategoriesQueryHandler : IQueryHandler<Query.GetCategoriesQuery, PagedResult<Response.CategoryResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository, 
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.CategoryResponse>>> Handle(Query.GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        //1. get list of categories
        var categories = _categoryRepository.FindAll();

        //2. paging
        var pagedCategories = await PagedResult<Domain.Entities.Category>.CreateAsync(
            categories, request.PageIndex, request.PageSize, cancellationToken);

        //3. mapping
        var result = _mapper.Map<PagedResult<Response.CategoryResponse>>(pagedCategories);
        
        return Result.Success(result);
    }
}