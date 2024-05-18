using Domain.Abstractions.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppEmployee : IdentityUser<Guid>, IEntity<Guid>, ISoftDeleted
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public bool IsDirector { get; set; }
    public bool IsHeadOfDepartment { get; set; }
    public Guid? ManagerId { get; set; }
    public string PasswordSalt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }

    // One-to-Many relationship - An employee can have many roles
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }


}