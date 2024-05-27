using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Employee;

public class DeleteEmployeeCommandHandler : ICommandHandler<Command.DeleteEmployeeCommand>
{
    private readonly IRepositoryBase<AppEmployee, Guid> _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteEmployeeCommandHandler(IRepositoryBase<AppEmployee, Guid> employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check employee existing
        var holderEmployee = await _employeeRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundException(request.Id);

        // Step 02: RaiseEvent
        holderEmployee.Delete();

        // Step 03: Peristence into DB
        _employeeRepository.Remove(holderEmployee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}