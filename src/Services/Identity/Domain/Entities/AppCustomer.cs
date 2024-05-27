using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppCustomer : AggregateRoot<Guid>, IEntity<Guid>, ISoftDeleted, IAuditable
{
    protected AppCustomer()
    {
    }

    private AppCustomer(Guid id, string firstName, string lastName, string fullName, string phoneNumber, string email, string passwordHash, string passwordSalt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public static AppCustomer Create(string firstName, string lastName, string phoneNumber, DateTime? dateofBirth, string? address, string email, string passwordHash, string passwordSalt)
    {
        string fullName = $"{firstName} {lastName}";

        if (fullName.Length > 40)
            throw new CustomerException.CustomerFieldException(nameof(FullName));

        var customer = new AppCustomer(Guid.NewGuid(), firstName, lastName, fullName, phoneNumber, email, passwordHash, passwordSalt);

        if (dateofBirth.HasValue)
            customer.AssignDateOfBirth(dateofBirth.Value);

        if (address is not null)
            customer.AssignAddress(address);

        customer.RaiseDomainEvent(new DomainEvent.CustomerCreated(Guid.NewGuid(), DateTimeOffset.UtcNow, customer.Id, customer.FullName, customer.Email, customer.PhoneNumber));

        return customer;
    }

    public void Delete()
    {
        RaiseDomainEvent(new DomainEvent.CustomerDeleted(Guid.NewGuid(), DateTime.UtcNow, Id));
    }

    private AppCustomer AssignAddress(string address)
    {
        Address = address;
        return this;
    }

    private AppCustomer AssignDateOfBirth(DateTime dateofbirth)
    {
        DateOfBirth = dateofbirth;
        return this;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime? DateOfBirth { get; private set; }

    // Address
    //public Address Address { get; set; }
    public string? Address { get; private set; }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }

    // Auditable
    public DateTimeOffset CreatedOnUtc { get; set; }

    public DateTimeOffset? ModifiedOnUtc { get; set; }

    // SoftDelete
    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOnUtc { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
}