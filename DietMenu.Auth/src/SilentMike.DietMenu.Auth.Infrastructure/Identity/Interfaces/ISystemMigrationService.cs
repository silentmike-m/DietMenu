namespace SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;

internal interface ISystemMigrationService
{
    Task MigrateSystemAsync(CancellationToken cancellationToken = default);
}
