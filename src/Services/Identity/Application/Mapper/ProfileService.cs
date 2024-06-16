using AutoMapper;
using Contracts.Abstractions.Paging;

namespace Application.Mapper;

public class ProfileService : Profile
{
    public ProfileService()
    {
        // Organzation
        CreateMap<
            Domain.Entities.Organization, 
            Contracts.Services.V1.Identity.Organization.Response.OrganizationDetailsResponse>();

        CreateMap<
            Domain.Entities.Organization,
            Contracts.Services.V1.Identity.Organization.Response.OrganizationResponse>();

        CreateMap<
            PagedResult<Domain.Entities.Organization>, 
            PagedResult<Contracts.Services.V1.Identity.Organization.Response.OrganizationResponse>>();

        // Customer
        CreateMap<
            Domain.Entities.AppCustomer, 
            Contracts.Services.V1.Identity.Customer.Response.CustomerDetailsResponse>();

        CreateMap<
            PagedResult<Domain.Entities.AppCustomer>,
            PagedResult<Contracts.Services.V1.Identity.Customer.Response.CustomerResponse>>();

        // Employee
        CreateMap<
            Domain.Entities.AppEmployee,
            Contracts.Services.V1.Identity.AppEmployee.Response.EmployeeDetailsResponse>();

        CreateMap<
            Domain.Entities.AppEmployee,
            Contracts.Services.V1.Identity.AppEmployee.Response.EmployeesResponse>()
            .ForMember(dest => dest.OrganizationName, opt => opt.MapFrom(src => src.Organization.Name));

        CreateMap<
            PagedResult<Domain.Entities.AppEmployee>,
            PagedResult<Contracts.Services.V1.Identity.AppEmployee.Response.EmployeesResponse>>();
    }
}