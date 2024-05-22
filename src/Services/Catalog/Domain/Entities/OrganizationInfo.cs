using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrganizationInfo : Entity<Guid>, ISoftDeleted
{
    private OrganizationInfo() { }

    private OrganizationInfo(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static OrganizationInfo Create(Guid id, string name)
    {
        var oranizationInfo = new OrganizationInfo(id, name);
        return oranizationInfo;
    }
    
    public void Delete()
    {
        IsDeleted = true;
    }

    public string Name { get; private set; }
    public bool IsDeleted { get; private set; }
}