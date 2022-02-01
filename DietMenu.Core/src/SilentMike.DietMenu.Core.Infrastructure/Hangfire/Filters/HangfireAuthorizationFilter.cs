namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Filters;

using System.Diagnostics.CodeAnalysis;
using global::Hangfire.Dashboard;

[ExcludeFromCodeCoverage]
internal sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
