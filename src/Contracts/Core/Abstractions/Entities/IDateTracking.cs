namespace Contracts.Core.Abstractions.Entities;

public interface IDateTracking
{
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
}