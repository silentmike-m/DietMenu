namespace SilentMike.DietMenu.Core.Infrastructure.Recipes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Events;

internal sealed class UpsertedRecipeHandler : INotificationHandler<UpsertedRecipe>
{
    private readonly ILogger<UpsertedRecipeHandler> logger;

    public UpsertedRecipeHandler(ILogger<UpsertedRecipeHandler> logger) => this.logger = logger;

    public async Task Handle(UpsertedRecipe notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("RecipeId", notification.Id)
        );

        this.logger.LogInformation("Upserted recipe");

        await Task.CompletedTask;
    }
}
