namespace Domain.Abstractions.Entities;

public interface ISoftDeleted
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}