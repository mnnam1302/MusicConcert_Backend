using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DependencyInjection.Options;

public record MessageBusOptions
{
    public int RetryLimit { get; init; }

    [Required, Timestamp]
    public TimeSpan InitialInterval { get; init; }

    [Required, Timestamp]
    public TimeSpan IntervalIncrement { get; init; }
}