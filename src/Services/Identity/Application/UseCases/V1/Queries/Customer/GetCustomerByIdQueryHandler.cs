using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Customer;

public class GetCustomerByIdQueryHandler : IQueryHandler<Query.GetCustomerByIdQuery, Response.CustomerDetailsResponse>
{
    private readonly IRepositoryBase<Domain.Entities.AppCustomer, Guid> _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(IRepositoryBase<AppCustomer, Guid> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.CustomerDetailsResponse>> Handle(Query.GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Check customer existsing?
        var customerHolder = await _customerRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new CustomerException.CustomerNotFoundException(request.Id);

        // Step 02: Mapping
        var result = _mapper.Map<Response.CustomerDetailsResponse>(customerHolder);

        return Result.Success(result);
    }
}