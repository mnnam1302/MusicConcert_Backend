
namespace Contracts.Core.Abstractions.Entities;

public abstract class EntitySoftDeleteBase<T> : EntityBase<T>, ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
}