
namespace Contracts.Core.Abstractions.Entities;

public abstract class EntityAuditBase<T> : EntityBase<T>, IEntityAuditBase<T>
{
    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}