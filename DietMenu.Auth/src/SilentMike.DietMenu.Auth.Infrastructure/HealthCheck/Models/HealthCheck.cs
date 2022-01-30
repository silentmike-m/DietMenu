namespace SilentMike.DietMenu.Auth.Infrastructure.HealthCheck.Models;

internal sealed record HealthCheck
{
    public IReadOnlyList<ComponentHealthCheck> HealthChecks { get; init; } = new List<ComponentHealthCheck>().AsReadOnly();
    public int HealthCheckDurationInMilliseconds { get; init; } = default;
    public string Status { get; init; } = string.Empty;
}
