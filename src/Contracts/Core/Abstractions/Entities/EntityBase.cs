namespace Contracts.Core.Abstractions.Entities;

public abstract class EntityBase<T> : IEntityBase<T>
{
    public T Id { get; set; }
}