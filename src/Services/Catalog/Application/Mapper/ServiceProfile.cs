using AutoMapper;
using Contracts.Abstractions.Paging;

namespace Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // Category
        CreateMap<Domain.Entities.Category, Contracts.Services.V1.Catalog.Category.Response.CategoryResponse>();
        // Remember don't need craete map here, uncomment will be error
        //CreateMap<List<Domain.Entities.Category>, List<Response.CategoryResponse>>(); 

        // Event
        CreateMap<Domain.Entities.Event, Contracts.Services.V1.Catalog.Event.Response.EventResponse>();
        CreateMap<PagedResult<Domain.Entities.Event>, PagedResult<Contracts.Services.V1.Catalog.Event.Response.EventResponse>>();

        // Ticket
        CreateMap<Domain.Entities.Ticket, Contracts.Services.V1.Catalog.Ticket.Response.TicketResponse>();
    }
}