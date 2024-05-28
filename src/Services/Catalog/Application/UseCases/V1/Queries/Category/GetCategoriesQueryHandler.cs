using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Category;

public class GetCategoriesQueryHandler : IQueryHandler<Query.GetCategoriesQuery, List<Response.CategoryResponse>>
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

    public async Task<Result<List<Response.CategoryResponse>>> Handle(Query.GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        //var categories = _categoryRepository.FindAll().ToList();
        var categories = await _categoryRepository.FindAllAsync(cancellationToken: cancellationToken);
        var result = _mapper.Map<List<Response.CategoryResponse>>(categories);

        //var result = new List<Response.CategoryResponse>();

        //foreach (var category in categories)
        //{
        //    var categoryResponse = new Response.CategoryResponse
        //    {
        //        Id = category.Id,
        //        Name = category.Name,
        //        Description = category.Description
        //    };
        //    result.Add(categoryResponse);
        //}

        return Result.Success(result);
    }
}