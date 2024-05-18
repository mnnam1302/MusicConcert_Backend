namespace Domain.Abstractions.Entities;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
}