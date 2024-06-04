using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Customer;

public class CustomerCreatedConsumerHandler : ICommandHandler<DomainEvent.CustomerCreated>
{
    private readonly IRepositoryBase<CustomerInfo, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerCreatedConsumerHandler(IRepositoryBase<CustomerInfo, Guid> customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.CustomerCreated request, CancellationToken cancellationToken)
    {
        // Step 01: Check customer existsing?
        var customerHolder = await _customerRepository.FindSingleAsync(x => x.CustomerId.Equals(request.Id), cancellationToken);

        if (customerHolder is not null)
            throw new CustomerInfoException.CustomerInfoAlreadyExistsException(request.Id);

        // Step 02: Create a new CustomerInfo
        //var customerInfo = CustomerInfo.Create(request.Id, request.FullName, request.Email, request.PhoneNumber);
        var customerInfo = new CustomerInfo(request.Id, request.FullName, request.Email, request.PhoneNumber);

        // Step 03: Persistence into DB
        _customerRepository.Add(customerInfo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}