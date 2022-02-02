namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

using System.ComponentModel;
using global::Hangfire;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;

internal sealed class ImportFamilyLibraries
{
    private const string INGREDIENTS_RESOURCE_NAME = "Ingredients.xlsx";

    private readonly IFileProvider fileProvider;
    private readonly ILogger<ImportFamilyLibraries> logger;
    private readonly IMediator mediator;

    public ImportFamilyLibraries(IFileProvider fileProvider, ILogger<ImportFamilyLibraries> logger, IMediator mediator)
    {
        this.fileProvider = fileProvider;
        this.logger = logger;
        this.mediator = mediator;
    }

    [DisplayName("Import libraries for family with id {0}")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid familyId)
    {
        this.logger.LogInformation("Import family libraries");

        await this.ImportIngredientTypes(familyId);

        await this.ImportMealTypes(familyId);

        await this.ImportIngredients(familyId);

        this.logger.LogInformation("Imported family libraries");
    }

    private async Task ImportIngredientTypes(Guid familyId)
    {
        var command = new ImportIngredientTypes
        {
            FamilyId = familyId,
        };

        _ = await this.mediator.Send(command, CancellationToken.None);
    }

    private async Task ImportMealTypes(Guid familyId)
    {
        var command = new ImportMealTypes
        {
            FamilyId = familyId,
        };

        _ = await this.mediator.Send(command, CancellationToken.None);
    }

    private async Task ImportIngredients(Guid familyId)
    {
        var ingredientsData = await this.GetIngredientsData();

        var command = new ImportIngredients
        {
            FamilyId = familyId,
            Payload = ingredientsData,
        };

        _ = await this.mediator.Send(command, CancellationToken.None);
    }

    private async Task<byte[]> GetIngredientsData()
    {
        await using var resourceStream = this.fileProvider.GetFileInfo(INGREDIENTS_RESOURCE_NAME).CreateReadStream();
        await using var memoryStream = new MemoryStream();
        await resourceStream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
