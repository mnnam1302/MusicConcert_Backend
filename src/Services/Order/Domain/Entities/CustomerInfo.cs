using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class CustomerInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    private CustomerInfo()
    {
    }

    public CustomerInfo(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
    }

    public Guid CustomerId { get; private set; } // Surrogate key => Should not use this property for Primary Key

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}