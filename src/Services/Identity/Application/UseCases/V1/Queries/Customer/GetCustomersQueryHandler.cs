using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Customer;

public class GetCustomersQueryHandler : IQueryHandler<Query.GetCustomersQuery, List<Response.CustomerResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.AppCustomer, Guid> _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IRepositoryBase<Domain.Entities.AppCustomer, Guid> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.CustomerResponse>>> Handle(Query.GetCustomersQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Get customers
        var customersHolder = await _customerRepository.FindAllAsync(cancellationToken: cancellationToken);

        // Step 02: Mapping
        var result = _mapper.Map<List<Response.CustomerResponse>>(customersHolder);

        return Result.Success(result);
    }
}