using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Catalog.Category;

public static class Command
{
    public record CreateCategoryCommand(string Name, string? Description) : ICommand;

    public record UpdateCategoryCommand(Guid Id, string Name, string? Description) : ICommand;

    public record DeleteCategoryCommand(Guid Id) : ICommand;
}