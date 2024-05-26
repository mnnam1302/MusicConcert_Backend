using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

//public class AppCustomer : IdentityUser<Guid>, IEntity<Guid>, ISoftDeleted, IAuditable
public class AppCustomer : AggregateRoot<Guid>, IEntity<Guid>, ISoftDeleted, IAuditable
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    // Address
    public Address Address { get; set; }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }

    // Auditable
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    // SoftDelete
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

    // Relationships
    public Guid RoleId { get; set; }
    public virtual AppRole AppRole { get; set; }
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
}