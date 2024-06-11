using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Employee;

public class CreateEmployeeCommandHandler : ICommandHandler<Command.CreateEmployeeCommand>
{
    private readonly IRepositoryBase<Domain.Entities.AppEmployee, Guid> _employeeRepository;
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashPasswordService _hashPasswordService;

    public CreateEmployeeCommandHandler(
        IRepositoryBase<Domain.Entities.AppEmployee, Guid> employeeRepository,
        IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository,
        IUnitOfWork unitOfWork,
        IHashPasswordService hashPasswordService)
    {
        _employeeRepository = employeeRepository;
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _hashPasswordService = hashPasswordService;
    }

    public async Task<Result> Handle(Command.CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        /*
            1. Check email existing
            2. Hash password
            3. Create employee
            4. Save employee
         */

        //1.
        var holderEmployee = await _employeeRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken);

        if (holderEmployee is not null)
            throw new EmployeeException.EmployeeAlreadyExistException(request.Email);

        if (request.OrganizationId.HasValue)
        {
            var holderOrganization = await _organizationRepository.FindSingleAsync(x => x.Id.Equals(request.OrganizationId), cancellationToken)
                ?? throw new OrganizationException.OrganizationNotFoundException(request.OrganizationId.Value);
        }

        //2.
        //var passwordSalt = _hashPasswordService.GenerateSalt();
        //var passwordHash = _hashPasswordService.HashPassword(request.Password, passwordSalt);

        var passwordHash = _hashPasswordService.HashPassword(request.Password);

        //3.
        var employee = Domain.Entities.AppEmployee.Create(request.FirstName, request.LastName, request.PhoneNumber, request.DateOfBirth, request.OrganizationId, request.Email, passwordHash);

        // Step 04: Persist employee
        _employeeRepository.Add(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}