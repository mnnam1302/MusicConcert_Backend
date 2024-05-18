using Domain.Abstractions.Entities;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Organization : Entity<Guid>, IAuditable, ISoftDeleted
{
    public string Name { get; set; }
    public string Industry { get; set; }
    public string Description { get; set; }
    public string Phone { get; set; }
    public string HomePage { get; set; }
    public string LogoUrl { get; set; }

    //public string Address { get; set; }
    //public string City { get; set; }
    //public string Region { get; set; }
    //public string Country { get; set; }
    //public string PostalCode { get; set; }

    // Apply Value Object
    public Address Address { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

    // Relationships
    //public virtual ICollection<AppEmployee>? Employees { get; set; }
}