namespace Domain.Abstractions.Entities;

public interface ISoftDelete
{
    public bool IsDeleted { get; }

    public DateTimeOffset DeletedOnUtc { get; }
}