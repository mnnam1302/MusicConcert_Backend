namespace Infrastructure.DependencyInjection.Options;

public record FireBaseOptions
{
    public string ApiKey { get; init; }
    public string StorageBucket { get; init; }
    public string AuthEmail { get; init; }
    public string AuthPassword { get; init; }
}