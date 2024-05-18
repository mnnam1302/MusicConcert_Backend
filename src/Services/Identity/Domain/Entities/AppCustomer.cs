using Domain.Abstractions.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppCustomer : IdentityUser<Guid>, IEntity<Guid>, ISoftDeleted, IAuditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Address
    public Address Address { get; set; }

    public string PasswordSalt { get; set; }

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