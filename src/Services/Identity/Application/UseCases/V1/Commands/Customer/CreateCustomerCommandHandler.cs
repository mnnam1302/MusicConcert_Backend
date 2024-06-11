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
        /*
            1. check email existing
            2. hash password
            3. create customer
            4. save db
         */

        //1.
        var holderCustomer = await _customerRepository.FindSingleAsync(x => x.Email == request.Email, cancellationToken);

        if (holderCustomer is not null)
            throw new CustomerException.CustomerAlreadyExistsException(request.Email);

        //2.
        var passwordHashed = _hashPasswordService.HashPassword(request.Password);

        //3.
        var customer = Domain.Entities.AppCustomer.Create(request.FirstName, request.LastName, request.PhoneNumber, request.DateOfBirth, request.Address, request.Email, passwordHashed);

        //4.
        _customerRepository.Add(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}