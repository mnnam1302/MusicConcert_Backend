using AutoMapper;
using Contracts.Abstractions.Paging;

namespace Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // Category
        CreateMap<
            Domain.Entities.Category, 
            Contracts.Services.V1.Catalog.Category.Response.CategoryResponse>();

        CreateMap<
            PagedResult<Domain.Entities.Category>, 
            PagedResult<Contracts.Services.V1.Catalog.Category.Response.CategoryResponse>>();

        // Event
        CreateMap<
            Domain.Entities.Event,
            Contracts.Services.V1.Catalog.Event.Response.EventDetailsReponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.OrganizationInfo.Name));

        CreateMap<
            Domain.Entities.Event, 
            Contracts.Services.V1.Catalog.Event.Response.EventResponse>();

        CreateMap<
            PagedResult<Domain.Entities.Event>, 
            PagedResult<Contracts.Services.V1.Catalog.Event.Response.EventResponse>>();

        // Ticket
        CreateMap<Domain.Entities.Ticket, Contracts.Services.V1.Catalog.Ticket.Response.TicketResponse>();
    }
}