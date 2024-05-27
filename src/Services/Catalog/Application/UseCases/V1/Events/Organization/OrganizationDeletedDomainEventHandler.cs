using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Organization;

public class OrganizationDeletedDomainEventHandler : ICommandHandler<DomainEvent.OrganizationDeleted>
{
    private readonly IRepositoryBase<OrganizationInfo, Guid> _organizationInfoRepository;
    private readonly IUnitOfWork _unitOfWork;
    public OrganizationDeletedDomainEventHandler(IRepositoryBase<OrganizationInfo, Guid> organizationInfoRepository, IUnitOfWork unitOfWork)
    {
        _organizationInfoRepository = organizationInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.OrganizationDeleted request, CancellationToken cancellationToken)
    {
        var organizationInfo = await _organizationInfoRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new OrganizationInfoException.OrganizationNotFoundException(request.Id);

        _organizationInfoRepository.Remove(organizationInfo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}