using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Application.UseCases.V1.Commands.Employee;

public class DeleteEmployeeCommandHandler : ICommandHandler<Command.DeleteEmployeeCommand>
{
    private readonly IRepositoryBase<AppEmployee, Guid> _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteEmployeeCommandHandler(IRepositoryBase<AppEmployee, Guid> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public Task<Result> Handle(Command.DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}