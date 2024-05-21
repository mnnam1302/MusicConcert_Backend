﻿using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrganizationInfo : Entity<Guid>, ISoftDeleted
{
    private OrganizationInfo() { }

    private OrganizationInfo(string name)
    {
        Name = name;
    }

    public static OrganizationInfo Create(string name)
    {
        var oranizationInfo = new OrganizationInfo(name);
        return oranizationInfo;
    }
    
    public void Delete()
    {
        IsDeleted = true;
    }

    public string Name { get; private set; }
    public bool IsDeleted { get; private set; }
}