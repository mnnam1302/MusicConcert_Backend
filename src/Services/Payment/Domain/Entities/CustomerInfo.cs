using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class CustomerInfo : Entity<Guid>, ISoftDeleted
{
    protected CustomerInfo()
    {
    }

    private CustomerInfo(Guid id, string fullName, string email, string phoneNumber)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        PhoneNumer = phoneNumber;
    }

    public static CustomerInfo Create(Guid id, string fullName, string email, string phoneNumber)
    {
        var customerInfo = new CustomerInfo(id, fullName, email, phoneNumber);
        return customerInfo;
    }

    public void Update(string fullName, string email, string phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PhoneNumer = phoneNumber;
    }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string PhoneNumer { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset DeletedOnUtc { get; set; }
}