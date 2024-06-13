using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.CustomerInfo;

public class CustomerDeletedConsumerHandler : ICommandHandler<DomainEvent.CustomerDeleted>
{
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerDeletedConsumerHandler(IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerInfoRepository, IUnitOfWork unitOfWork)
    {
        _customerInfoRepository = customerInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.CustomerDeleted request, CancellationToken cancellationToken)
    {
        //1. check customer info exists?
        var customerHolder = await _customerInfoRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new CustomerInfoException.CustomerInfoNotFoundException(request.Id);

        //2. remove customer info
        _customerInfoRepository.Remove(customerHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}