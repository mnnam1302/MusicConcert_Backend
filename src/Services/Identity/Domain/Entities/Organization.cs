using Contracts.Services.V1.Identity;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.ValueObjects;
using System.Runtime.CompilerServices;

namespace Domain.Entities;

public class Organization : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    /*
     * Why need parameterless constructor here?
     * In your case, the Organization class has a constructor with parameters, including an address parameter that corresponds to the Address property. However, EF Core cannot bind this parameter because Address is an owned entity type, not a simple property that is mapped to a database column.
     */

    private Organization() { }

    public Organization(Guid id, string name, string industry, string phone, string homePage, Address address)
    {
        Id = id;
        Name = name;
        Industry = industry;
        Phone = phone;
        HomePage = homePage;
        Address = address;
    }

    public static Organization Create(string name, string industry, string? description, string phone, string homepage, string? logoUrl, string street, string city, string state, string country, string zipCode)
    {
        var organization = new Organization(Guid.NewGuid(), name, industry, phone, homepage, 
            new Address(street, city, state, country, zipCode));

        if (!string.IsNullOrEmpty(description))
            organization.AssignDescription(description);

        if (!string.IsNullOrEmpty(logoUrl))
            organization.AssignLogoUrl(logoUrl);

        organization.RaiseDomainEvent(new DomainEvent.OrganizationCreated(
            Guid.NewGuid(), 
            DateTime.UtcNow, 
            organization.Id, 
            organization.Name));

        return organization;
    }

    private Organization AssignDescription(string description)
    {
        Description = description;
        return this;
    }

    private Organization AssignLogoUrl(string logoUrl)
    {
        LogoUrl = logoUrl;
        return this;
    }

    public string Name { get; private set; }
    public string Industry { get; private set; }
    public string? Description { get; private set; }
    public string Phone { get; private set; }
    public string HomePage { get; private set; }
    public string? LogoUrl { get; private set; }

    //public string Street { get; private set; }
    //public string City { get; private set; }
    //public string State { get; private set; }
    //public string Country { get; private set; }
    //public string ZipCode { get; private set; }

    // Apply Value Object
    // Note: no nullable here => Read more notion
    // Me: Strreet-null, City-null, ZipCode-null, State-abcdhb => Null or No null => Confuse
    // Check all properties that are all null => Null
    public Address Address { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}