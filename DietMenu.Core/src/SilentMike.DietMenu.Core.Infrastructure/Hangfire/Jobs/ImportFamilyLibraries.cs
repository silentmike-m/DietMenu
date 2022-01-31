namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

using System.ComponentModel;
using global::Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;

internal sealed class ImportFamilyLibraries
{
    private readonly ILogger<ImportFamilyLibraries> logger;
    private readonly IMediator mediator;

    public ImportFamilyLibraries(ILogger<ImportFamilyLibraries> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [DisplayName("Import libraries for family with id {0}")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid familyId)
    {
        this.logger.LogInformation("Import family libraries");

        await this.ImportMealTypes(familyId);

        this.logger.LogInformation("Imported family libraries");
    }

    private async Task ImportMealTypes(Guid familyId)
    {
        var command = new ImportMealTypes
        {
            FamilyId = familyId,
        };

        _ = await this.mediator.Send(command, CancellationToken.None);
    }
}
