using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.CustomerInfo;

public class CustomerCreatedConsumerHandler : ICommandHandler<DomainEvent.CustomerCreated>
{
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerCreatedConsumerHandler(IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerInfoRepository, IUnitOfWork unitOfWork)
    {
        _customerInfoRepository = customerInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.CustomerCreated request, CancellationToken cancellationToken)
    {
        //1. check customer info exists?
        var customerHolder = await _customerInfoRepository.FindByIdAsync(request.Id, cancellationToken);

        if (customerHolder is not null)
            throw new CustomerInfoException.CustomerInfoAlreadyExistsException(request.Id);

        //2. create customer info
        var customerInfo = new Domain.Entities.CustomerInfo(request.Id, request.FullName, request.Email, request.PhoneNumber);

        //3. persist into database
        _customerInfoRepository.Add(customerInfo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}