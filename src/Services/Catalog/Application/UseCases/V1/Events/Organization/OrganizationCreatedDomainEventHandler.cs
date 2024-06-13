using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Organization;

public class OrganizationCreatedDomainEventHandler : ICommandHandler<DomainEvent.OrganizationCreated>
{
    private readonly IRepositoryBase<OrganizationInfo, Guid> _organizationInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationCreatedDomainEventHandler(IRepositoryBase<OrganizationInfo, Guid> organizationInfoRepository, IUnitOfWork unitOfWork)
    {
        _organizationInfoRepository = organizationInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.OrganizationCreated request, CancellationToken cancellationToken)
    {
        // Step 01: Check if the organization already exists
        var organizationHolder = await _organizationInfoRepository.FindByIdAsync(request.Id, cancellationToken);

        if (organizationHolder is not null)
            throw new OrganizationInfoException.OrganizaitonInfoAlreadyExistsException(request.Id);

        //var organizationInfo = OrganizationInfo.Create(request.Id, request.Name);
        var organizationInfo = new OrganizationInfo(request.Id, request.Name);

        _organizationInfoRepository.Add(organizationInfo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}