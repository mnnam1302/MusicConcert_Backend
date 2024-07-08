using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;

namespace Contracts.Services.V1.Payment.Invoice;

public class Query
{
    public record GetInvoicesByCustomerIdQuery(
        Guid CustomerId,
        int PageIndex,
        int PageSize) : IQuery<PagedResult<Response.InvoiceResponse>>;
}