namespace Contracts.Services.V1.Catalog.Category;

public static class Response
{
    public record CategoryResponse(Guid Id, string Name, string? Description);
}