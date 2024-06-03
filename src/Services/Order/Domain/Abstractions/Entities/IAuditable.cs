namespace Domain.Abstractions.Entities;

public interface IAuditable
{
    public DateTimeOffset CreatedOnUtc { get; }
    public DateTimeOffset? ModifiedOnUtc { get; }
}