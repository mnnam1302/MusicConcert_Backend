using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Organization;

public class DeleteOrganizationCommandHandler : ICommandHandler<Command.DeleteOrganizationCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationCommandHandler(IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository, IUnitOfWork unitOfWork)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check organization existing
        var holderOrganization = await _organizationRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new OrganizationException.OrganizationNotFoundException(request.Id);

        // Step 02: RaiseEvent
        holderOrganization.Delete();

        // Step 03: Peristence into DB
        _organizationRepository.Remove(holderOrganization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}