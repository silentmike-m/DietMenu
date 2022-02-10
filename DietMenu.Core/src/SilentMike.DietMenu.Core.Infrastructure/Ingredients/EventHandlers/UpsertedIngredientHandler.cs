namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;

internal sealed class UpsertedIngredientHandler : INotificationHandler<UpsertedIngredient>
{
    private readonly ILogger<UpsertedIngredientHandler> logger;

    public UpsertedIngredientHandler(ILogger<UpsertedIngredientHandler> logger)
        => this.logger = logger;

    public async Task Handle(UpsertedIngredient notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("IngredientId", notification.Id)
        );

        this.logger.LogInformation("Upserted ingredient");

        await Task.CompletedTask;
    }
}
