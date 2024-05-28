namespace Contracts.Services.V1.Catalog.Category;

public static class Response
{
    //public record CategoryResponse(Guid Id, string Name, string? Description);

    public record CategoryResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
    }
}