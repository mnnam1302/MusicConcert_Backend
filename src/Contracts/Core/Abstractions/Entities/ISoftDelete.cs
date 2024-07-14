namespace Contracts.Core.Abstractions.Entities;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedDate { get; set; }
}