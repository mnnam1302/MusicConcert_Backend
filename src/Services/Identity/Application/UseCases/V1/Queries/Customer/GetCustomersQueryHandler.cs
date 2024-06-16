using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Customer;

public class GetCustomersQueryHandler : IQueryHandler<Query.GetCustomersQuery, PagedResult<Response.CustomerResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.AppCustomer, Guid> _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IRepositoryBase<Domain.Entities.AppCustomer, Guid> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.CustomerResponse>>> Handle(Query.GetCustomersQuery request, CancellationToken cancellationToken)
    {
        //1. get all customers
        var customersHolder = _customerRepository.FindAll();

        //2. Page
        var customers = await PagedResult<Domain.Entities.AppCustomer>.CreateAsync(
            customersHolder, 
            request.PageIndex, 
            request.PageSize, 
            cancellationToken);

        // Step 02: Mapping
        var result = _mapper.Map<PagedResult<Response.CustomerResponse>>(customers);

        return Result.Success(result);
    }
}