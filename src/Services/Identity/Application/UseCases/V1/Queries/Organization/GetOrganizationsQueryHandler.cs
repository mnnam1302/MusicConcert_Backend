using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Organization;

public class GetOrganizationsQueryHandler : IQueryHandler<Query.GetOrganizationsQuery, PagedResult<Response.OrganizationResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IMapper _mapper;

    public GetOrganizationsQueryHandler(IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<PagedResult<Response.OrganizationResponse>>> Handle(Query.GetOrganizationsQuery request, CancellationToken cancellationToken)
    {
        //1. Get organizations
        var organizationsHolder = _organizationRepository.FindAll();

        //2. Paging
        var organizations = await PagedResult<Domain.Entities.Organization>.CreateAsync(
            organizationsHolder, 
            request.PageIndex, 
            request.PageSize, 
            cancellationToken);

        //3. Mapping
        var result = _mapper.Map<PagedResult<Response.OrganizationResponse>>(organizations);

        return Result.Success(result);
    }
}