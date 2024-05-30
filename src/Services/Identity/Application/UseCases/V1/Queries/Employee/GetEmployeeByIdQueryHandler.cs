using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Employee;

public class GetEmployeeByIdQueryHandler : IQueryHandler<Query.GetEmployeeByIdQuery, Response.EmployeeDetailsResponse>
{
    private readonly IRepositoryBase<Domain.Entities.AppEmployee, Guid> _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IRepositoryBase<AppEmployee, Guid> employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.EmployeeDetailsResponse>> Handle(Query.GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Check employee existing?
        var employeeHolder = await _employeeRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundException(request.Id);

        // Step 02: Mapping
        var result = _mapper.Map<Response.EmployeeDetailsResponse>(employeeHolder);

        return Result.Success(result);
    }
}