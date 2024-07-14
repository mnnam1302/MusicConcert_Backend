namespace Contracts.Core.Abstractions.Entities;

public interface IEntityBase<T>
{
    public T Id { get; set; }
}