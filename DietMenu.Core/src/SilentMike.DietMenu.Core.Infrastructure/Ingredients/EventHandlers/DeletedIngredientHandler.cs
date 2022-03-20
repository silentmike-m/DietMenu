namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;

internal sealed class DeletedIngredientHandler : INotificationHandler<DeletedIngredient>
{
    private readonly ILogger<DeletedIngredientHandler> logger;

    public DeletedIngredientHandler(ILogger<DeletedIngredientHandler> logger) => this.logger = logger;

    public async Task Handle(DeletedIngredient notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("RecipeId", notification.Id)
        );

        this.logger.LogInformation("Deleted ingredient");

        await Task.CompletedTask;
    }
}