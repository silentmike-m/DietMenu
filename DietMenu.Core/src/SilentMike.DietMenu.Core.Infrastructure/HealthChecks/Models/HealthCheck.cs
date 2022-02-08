namespace SilentMike.DietMenu.Core.Infrastructure.HealthChecks.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal sealed record HealthCheck
{
    public IReadOnlyList<ComponentHealthCheck> HealthChecks { get; init; } = new List<ComponentHealthCheck>().AsReadOnly();
    public int HealthCheckDurationInMilliseconds { get; init; } = default;
    public string Status { get; init; } = string.Empty;
}
