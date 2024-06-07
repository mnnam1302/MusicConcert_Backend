using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrderInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    public OrderInfo()
    {
    }

    public OrderInfo(Guid orderId)
    {
        Id = orderId;
    }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}