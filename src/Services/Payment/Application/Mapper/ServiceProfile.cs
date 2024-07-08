using AutoMapper;
using Contracts.Abstractions.Paging;

namespace Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<
            Domain.Entities.Invoice, 
            Contracts.Services.V1.Payment.Invoice.Response.InvoiceResponse>();

        CreateMap<
            PagedResult<Domain.Entities.Invoice>, 
            PagedResult<Contracts.Services.V1.Payment.Invoice.Response.InvoiceResponse>>();
    }
}