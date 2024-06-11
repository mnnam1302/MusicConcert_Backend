namespace ApiGateway.DependencyInjection.Options;

public record JwtOptions
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecretKey { get; init; }
    public short ExpireMin { get; init; }
}