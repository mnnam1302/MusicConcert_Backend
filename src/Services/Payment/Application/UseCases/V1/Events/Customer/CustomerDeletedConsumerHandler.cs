using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Customer;

public class CustomerDeletedConsumerHandler : ICommandHandler<DomainEvent.CustomerDeleted>
{
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerDeletedConsumerHandler(
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.CustomerDeleted request, CancellationToken cancellationToken)
    {
        // Step 01: check customer existsing?
        var customerHolder = await _customerRepository.FindSingleAsync(x => x.CustomerId.Equals(request.Id), cancellationToken)
            ?? throw new CustomerInfoException.CustomerInfoNotFoundException(request.Id);

        // Step 02: Remove and persistence into database
        _customerRepository.Remove(customerHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}