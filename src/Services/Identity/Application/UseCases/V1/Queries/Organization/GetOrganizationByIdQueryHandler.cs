using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Organization;

public class GetOrganizationByIdQueryHandler : IQueryHandler<Query.GetOrganizaitionByIdQuery, Response.OrganizationDetailsResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IMapper _mapper;

    public GetOrganizationByIdQueryHandler(IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.OrganizationDetailsResponse>> Handle(Query.GetOrganizaitionByIdQuery request, CancellationToken cancellationToken)
    {
        // Step 01: check organization existsing?
        var organizationHolder = await _organizationRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new OrganizationException.OrganizationNotFoundException(request.Id);

        // Step 02: Mapper
        var result = _mapper.Map<Response.OrganizationDetailsResponse>(organizationHolder);

        return Result.Success(result);
    }
}