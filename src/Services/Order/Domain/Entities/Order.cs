using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class Order : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{

    public Guid CustomerInfoId { get; set; }
    public virtual  CustomerInfo CustomerInfo { get; set; } // must belong one CustomerInfo

    public virtual List<OrderDetails> OrderDetails { get; private set; } = new();

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}