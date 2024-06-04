using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class CustomerInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    protected CustomerInfo()
    {
    }

    public CustomerInfo(Guid customerId, string fullName, string email, string phoneNumber)
    {
        Id = Guid.NewGuid(); // Surrogate key in Service Payment
        CustomerId = customerId;
        FullName = fullName;
        Email = email;
        PhoneNumer = phoneNumber;
    }
    
    public static CustomerInfo Create(Guid customerId, string fullName, string email, string phoneNumber)
    {
        var customerInfo = new CustomerInfo(customerId, fullName, email, phoneNumber);
        return customerInfo;
    }

    public void Update(string fullName, string email, string phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PhoneNumer = phoneNumber;
    }

    public Guid CustomerId { get; private set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumer { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}