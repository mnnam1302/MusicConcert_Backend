using AutoMapper;

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

        // Customer
        CreateMap<
            Domain.Entities.AppCustomer, 
            Contracts.Services.V1.Identity.Customer.Response.CustomerDetailsResponse>();
        CreateMap<
            Domain.Entities.AppCustomer, 
            Contracts.Services.V1.Identity.Customer.Response.CustomerResponse>();

        // Employee
        CreateMap<
            Domain.Entities.AppEmployee,
            Contracts.Services.V1.Identity.AppEmployee.Response.EmployeeDetailsResponse>();
    }
}