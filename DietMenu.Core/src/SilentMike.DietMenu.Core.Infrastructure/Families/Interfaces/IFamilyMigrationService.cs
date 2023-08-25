namespace SilentMike.DietMenu.Core.Infrastructure.Families.Interfaces;

internal interface IFamilyMigrationService
{
    Task ImportAsync(Guid familyId, CancellationToken cancellationToken = default);
}
