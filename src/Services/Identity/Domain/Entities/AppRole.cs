using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppRole : IdentityRole<Guid>
{
    public string Description { get; set; }

    public string RoleCode { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
}