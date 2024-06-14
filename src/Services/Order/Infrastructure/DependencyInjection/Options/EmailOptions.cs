namespace Infrastructure.DependencyInjection.Options;

public record EmailOptions
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string From { get; init; }
    public string DisplayName { get; init; }
}