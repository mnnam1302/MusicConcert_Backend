using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Payment.Invoice;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace Application.UseCases.V1.Queries.Invoice;

public class GetInvoicesByCustomerIdQueryHandler
    : IQueryHandler<Query.GetInvoicesByCustomerIdQuery, PagedResult<Response.InvoiceResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IRepositoryBase<Domain.Entities.Invoice, Guid> _invoiceRepository;
    private readonly IMapper _mapper;

    public GetInvoicesByCustomerIdQueryHandler(
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IRepositoryBase<Domain.Entities.Invoice, Guid> invoiceRepository,
        IMapper mapper)
    {
        _customerRepository = customerRepository;
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.InvoiceResponse>>> Handle(Query.GetInvoicesByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        /*
         * 1. Check if the customer exists
         * 2. Get the invoices by customer id
         * 3. Pagination
         * 4. Map the invoices to the response
         */

        // 1.
        var foundCustomerInfo = await _customerRepository.FindByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException("Customer was not found.");

        // 2.
        var invoices = _invoiceRepository.FindAll(x => 
                                                        x.CustomerInfoId == foundCustomerInfo.Id 
                                                        && x.Status != InvoiceStatus.Cancelled);

        // 3.
        var pagedInvoices = await PagedResult<Domain.Entities.Invoice>.CreateAsync(invoices, request.PageIndex, request.PageSize, cancellationToken);

        // 4.
        var result = _mapper.Map<PagedResult<Response.InvoiceResponse>>(pagedInvoices);

        return Result.Success(result);
    }
}