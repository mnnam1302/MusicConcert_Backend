using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Organization;

public class GetOrganizationsQueryHandler : IQueryHandler<Query.GetOrganizationsQuery, List<Response.OrganizationResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IMapper _mapper;

    public GetOrganizationsQueryHandler(IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.OrganizationResponse>>> Handle(Query.GetOrganizationsQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Get organizations
        var organizationsHolder = await _organizationRepository.FindAllAsync(cancellationToken: cancellationToken);

        // Step 02: Mapper
        var result = _mapper.Map<List<Response.OrganizationResponse>>(organizationsHolder);

        return Result.Success(result);
    }
}