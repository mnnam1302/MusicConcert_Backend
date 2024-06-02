using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class CustomerInfo : Entity<Guid>, ISoftDelete
{
    public bool IsDeleted { get; set; }

    public DateTimeOffset DeletedOnUtc { get; set; }
}