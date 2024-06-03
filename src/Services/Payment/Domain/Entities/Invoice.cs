using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class Invoice : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    public decimal SubTotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Tax { get; private set; }
    public decimal TotalPrice { get; set; }
    public Guid? CustomerInfoId { get; set; }
    public virtual CustomerInfo? CustomerInfo { get; set; }
    public Guid? OrderInfoId { get; set; }
    public virtual OrderInfo? OrderInfo { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}