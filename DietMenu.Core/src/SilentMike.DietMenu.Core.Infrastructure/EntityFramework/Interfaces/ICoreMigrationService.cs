namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Interfaces;

public interface ICoreMigrationService
{
    Task MigrateCoreAsync(CancellationToken cancellationToken = default);
}
