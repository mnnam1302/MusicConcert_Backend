using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrganizationInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    protected OrganizationInfo() { }

    public OrganizationInfo(Guid organizationId, string name)
    {
        Id = Guid.NewGuid();
        OrganizaitonId = organizationId;
        Name = name;
    }

    public Guid OrganizaitonId { get; private set; }
    
    public string Name { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}