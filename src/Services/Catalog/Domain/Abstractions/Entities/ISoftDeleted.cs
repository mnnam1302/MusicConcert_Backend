namespace Domain.Abstractions.Entities;

public interface ISoftDeleted
{
    public bool IsDeleted { get; }
}