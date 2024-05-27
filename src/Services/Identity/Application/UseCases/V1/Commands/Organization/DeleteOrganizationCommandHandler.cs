using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Organization;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Organization;

public class DeleteOrganizationCommandHandler : ICommandHandler<Command.DeleteOrganizationCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationId;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrganizationCommandHandler(IRepositoryBase<Domain.Entities.Organization, Guid> organizationId, IUnitOfWork unitOfWork)
    {
        _organizationId = organizationId;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        // Check organization existing?
        var holderOrganization = await _organizationId.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new OrganizationException.OrganizationNotFoundException(request.Id);

        // DDD -> RaiseEvent
        holderOrganization.Delete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}