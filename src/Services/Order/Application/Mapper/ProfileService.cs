using AutoMapper;

namespace Application.Mapper;

public class ProfileService : Profile
{
    public ProfileService()
    {
        CreateMap<Domain.Entities.OrderDetails, Contracts.Services.V1.Order.Response.OrderDetailsResponse>();

        CreateMap<Domain.Entities.Order, Contracts.Services.V1.Order.Response.OrderResponse>();
            //.ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
    }
}