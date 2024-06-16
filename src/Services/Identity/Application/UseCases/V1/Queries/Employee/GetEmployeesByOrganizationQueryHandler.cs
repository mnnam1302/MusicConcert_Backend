using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Employee;

public class GetEmployeesByOrganizationQueryHandler
    : IQueryHandler<Query.GetEmployeesByOrganizationQuery, PagedResult<Response.EmployeesResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.AppEmployee, Guid> _employeeRepository;
    private readonly IRepositoryBase<Domain.Entities.Organization, Guid> _organizationRepository;
    private readonly IMapper _mapper;

    public GetEmployeesByOrganizationQueryHandler(
        IRepositoryBase<Domain.Entities.AppEmployee, Guid> employeeRepository,
        IRepositoryBase<Domain.Entities.Organization, Guid> organizationRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.EmployeesResponse>>> Handle(Query.GetEmployeesByOrganizationQuery request, CancellationToken cancellationToken)
    {
        //1. check oranization if request.OrganizationId exists
        if (request.OrganizationId.HasValue)
        {
            var foundOrganization = _organizationRepository.FindByIdAsync(request.OrganizationId.Value)
                ?? throw new OrganizationException.OrganizationNotFoundException(request.OrganizationId.Value);
        }

        //2. get employees by organization
        var employees = request.OrganizationId.HasValue
            ? _employeeRepository.FindAll(x => x.OrganizationId == request.OrganizationId, x => x.Organization)
            : _employeeRepository.FindAll(includeProperties: x => x.Organization);

        //3. Paging
        var pagedEmployees = await PagedResult<Domain.Entities.AppEmployee>.CreateAsync(
            employees, 
            request.PageIndex, 
            request.PageSize,
            cancellationToken);

        //4. Mapping
        var result = _mapper.Map<PagedResult<Response.EmployeesResponse>>(pagedEmployees);

        return Result.Success(result);
    }
}