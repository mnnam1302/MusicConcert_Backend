using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Exceptions;
using MassTransit.SagaStateMachine;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Domain.Entities;

public class AppEmployee : AggregateRoot<Guid>, IEntity<Guid>, ISoftDeleted
{
    protected AppEmployee()
    {
    }

    private AppEmployee(Guid id, string firstName, string lastName, string fullName, string phoneNumber, string email, string passwordHash)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static AppEmployee Create(string firstName, string lastName, string phoneNumber, DateTime? dateofBirth, Guid? organizationId, string email, string passwordHash)
    {
        string fullName = $"{firstName} {lastName}";

        if (fullName.Length > 40)
            throw new EmployeeException.EmployeeFieldException(nameof(FullName));

        var employee = new AppEmployee(Guid.NewGuid(), firstName, lastName, fullName, phoneNumber, email, passwordHash);

        if (dateofBirth.HasValue)
            employee.AssignDateOfBirth(dateofBirth.Value);

        if (organizationId.HasValue)
            employee.AssignOrganization(organizationId.Value);

        //employee.RaiseDomainEvent(new DomainEvent.EmployeeCreated(Guid.NewGuid(), DateTimeOffset.UtcNow, employee.Id, employee.FirstName, employee.LastName));

        return employee;
    }

    public void Delete()
    {
        //RaiseDomainEvent(new DomainEvent.EmployeeDeleted(Guid.NewGuid(), DateTime.UtcNow, Id));
    }

    private AppEmployee AssignDateOfBirth(DateTime dateOfBirth)
    {
        DateOfBirth = dateOfBirth;
        return this;
    }

    private AppEmployee AssignOrganization(Guid organizationId)
    {
        OrganizationId = organizationId;
        return this;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    public bool IsDirector { get; private set; }
    public bool IsHeadOfDepartment { get; private set; }
    public Guid? ManagerId { get; private set; }
    
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }

   // Relationships
    public Guid? OrganizationId { get; set; }
    public virtual Organization? Organization { get; set; } // An employee only work for an organization

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } // An employee can have many roles
}