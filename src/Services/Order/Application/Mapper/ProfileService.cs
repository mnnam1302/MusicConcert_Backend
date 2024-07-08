using AutoMapper;

namespace Application.Mapper;

public class ProfileService : Profile
{
    public ProfileService()
    {
        CreateMap<Domain.Entities.Order, Contracts.Services.V1.Order.Response.OrderResponse>();
        
        CreateMap<Domain.Entities.OrderDetails, Contracts.Services.V1.Order.Response.OrderDetailsResponse>()
            .ForMember(dest => dest.TicketName, opt => opt.MapFrom(src => src.TicketInfo.Name))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity));

    }
}