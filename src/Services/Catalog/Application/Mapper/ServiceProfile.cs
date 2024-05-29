using AutoMapper;

namespace Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Domain.Entities.Category, Contracts.Services.V1.Catalog.Category.Response.CategoryResponse>();
        // Remember don't need craete map here, uncomment will be error
        //CreateMap<List<Domain.Entities.Category>, List<Response.CategoryResponse>>(); 

        CreateMap<Domain.Entities.Event, Contracts.Services.V1.Catalog.Event.Response.EventResponse>();
    }
}