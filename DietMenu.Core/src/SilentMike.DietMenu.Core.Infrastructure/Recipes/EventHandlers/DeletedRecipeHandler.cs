namespace SilentMike.DietMenu.Core.Infrastructure.Recipes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Events;

internal sealed class DeletedRecipeHandler : INotificationHandler<DeletedRecipe>
{
    private readonly ILogger<DeletedRecipeHandler> logger;

    public DeletedRecipeHandler(ILogger<DeletedRecipeHandler> logger) => this.logger = logger;

    public async Task Handle(DeletedRecipe notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("RecipeId", notification.Id)
        );

        this.logger.LogInformation("Deleted recipe");

        await Task.CompletedTask;
    }
}