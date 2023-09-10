namespace SilentMike.DietMenu.Proxy.Infrastructure.Common.Services;

using SilentMike.DietMenu.Proxy.Infrastructure.Common.Interfaces;

internal sealed class DateTimeService : IDateTimeService
{
    public DateTime GetNow()
        => DateTime.UtcNow;
}
