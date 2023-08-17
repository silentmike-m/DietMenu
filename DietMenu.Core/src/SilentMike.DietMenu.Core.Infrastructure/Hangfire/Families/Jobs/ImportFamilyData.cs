namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs;

using System.ComponentModel;
using global::Hangfire;
using SilentMike.DietMenu.Core.Infrastructure.Families.Interfaces;

internal sealed class ImportFamilyData
{
    private readonly IFamilyMigrationService familyMigrationService;

    public ImportFamilyData(IFamilyMigrationService familyMigrationService)
        => this.familyMigrationService = familyMigrationService;

    [DisplayName("Import data for family with id {0}"), AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid familyId)
    {
        await this.familyMigrationService.ImportAsync(familyId, CancellationToken.None);
    }
}
