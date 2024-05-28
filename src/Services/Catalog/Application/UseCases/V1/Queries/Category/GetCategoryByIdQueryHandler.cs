using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Category;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Category;

public class GetCategoryByIdQueryHandler : IQueryHandler<Query.GetCategoryByIdQuery, Response.CategoryResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository, 
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.CategoryResponse>> Handle(Query.GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var holderCategory = await _categoryRepository.FindByIdAsync(request.CategoryId, cancellationToken)
            ?? throw new CategoryException.CategoryNotFoundException(request.CategoryId);

        var result = _mapper.Map<Response.CategoryResponse>(holderCategory);
        return result;
    }
}