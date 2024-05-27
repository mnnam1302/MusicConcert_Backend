﻿using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrganizationInfo : Entity<Guid>, ISoftDeleted
{
    protected OrganizationInfo() { }

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

    public string Name { get; private set; }
    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOnUtc { get; set; }
}