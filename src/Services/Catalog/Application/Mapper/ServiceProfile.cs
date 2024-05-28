using AutoMapper;
using Contracts.Services.V1.Catalog.Category;

namespace Application.Mapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Domain.Entities.Category, Response.CategoryResponse>();
        // Remember don't need craete map here, uncomment will be error
        //CreateMap<List<Domain.Entities.Category>, List<Response.CategoryResponse>>(); 
    }
}