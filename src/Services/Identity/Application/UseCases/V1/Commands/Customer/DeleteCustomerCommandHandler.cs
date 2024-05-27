using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Customer;

public class DeleteCustomerCommandHandler : ICommandHandler<Command.DeleteCustomerCommand>
{
    private readonly IRepositoryBase<AppCustomer, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteCustomerCommandHandler(IRepositoryBase<AppCustomer, Guid> customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check customer existing?
        var holderCustomer = await _customerRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new CustomerException.CustomerNotFoundException(request.Id);

        // Step 02: RaiseEvent
        holderCustomer.Delete();

        // Step 03: Persistence into DB
        _customerRepository.Remove(holderCustomer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}