namespace Domain.Abstractions.Entities;

public interface IAuditable
{
    DateTimeOffset CreatedOnUtc { get; set; }

    DateTimeOffset? ModifiedOnUtc { get; set; }
}