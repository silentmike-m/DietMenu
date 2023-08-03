namespace SilentMike.DietMenu.Auth.Infrastructure.Date.Services;

using SilentMike.DietMenu.Auth.Application.Common;

internal sealed class DateTimeService : IDateTimeService
{
    public DateTime GetNow()
        => DateTime.UtcNow;
}
