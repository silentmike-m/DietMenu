namespace SilentMike.DietMenu.Core.Infrastructure.IngredientTypes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Events;

internal sealed class UpsertedIngredientTypesHandler : INotificationHandler<UpsertedIngredientTypes>
{
    private readonly ILogger<UpsertedIngredientTypesHandler> logger;

    public UpsertedIngredientTypesHandler(ILogger<UpsertedIngredientTypesHandler> logger)
        => this.logger = logger;

    public async Task Handle(UpsertedIngredientTypes notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("Ids", notification.Ids)
        );

        this.logger.LogInformation("Upserted ingredient types");

        await Task.CompletedTask;
    }
}
