using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Customer;

public class CreateCustomerCommandHandler : ICommandHandler<Command.CreateCustomerCommand>
{
    private readonly IRepositoryBase<Domain.Entities.AppCustomer, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashPasswordService _hashPasswordService;

    public CreateCustomerCommandHandler(
        IRepositoryBase<Domain.Entities.AppCustomer, Guid> customerRepository,
        IUnitOfWork unitOfWork,
        IHashPasswordService hashPasswordService)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _hashPasswordService = hashPasswordService;
    }

    public async Task<Result> Handle(Command.CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        // Step 01: check email existing?
        var holderCustomer = await _customerRepository.FindSingleAsync(x => x.Email == request.Email, cancellationToken);

        if (holderCustomer is not null)
            throw new CustomerException.CustomerAlreadyExistsException(request.Email);

        // Step 02: Hash password
        var passwordSalt = _hashPasswordService.GenerateSalt();
        var passwordHash = _hashPasswordService.HashPassword(request.Password, passwordSalt);

        // Step 03: Create customer
        var customer = Domain.Entities.AppCustomer.Create(request.FirstName, request.LastName, request.PhoneNumber, request.DateOfBirth, request.Address, request.Email, passwordHash, passwordSalt);

        // Step 04: Persist db
        _customerRepository.Add(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}