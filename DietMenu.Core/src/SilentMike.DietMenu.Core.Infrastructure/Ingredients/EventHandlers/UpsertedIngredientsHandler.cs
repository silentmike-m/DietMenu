namespace SilentMike.DietMenu.Core.Infrastructure.Ingredients.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;

internal sealed class UpsertedIngredientsHandler : INotificationHandler<UpsertedIngredients>
{
    private readonly ILogger<UpsertedIngredientsHandler> logger;

    public UpsertedIngredientsHandler(ILogger<UpsertedIngredientsHandler> logger)
        => this.logger = logger;

    public async Task Handle(UpsertedIngredients notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("Ids", notification.Ids)
        );

        this.logger.LogInformation("Upserted ingredients");

        await Task.CompletedTask;
    }
}
