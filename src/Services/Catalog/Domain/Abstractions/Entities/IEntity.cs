namespace Domain.Abstractions.Entities;

public interface IEntity<T>
{
    public T Id { get; }
}