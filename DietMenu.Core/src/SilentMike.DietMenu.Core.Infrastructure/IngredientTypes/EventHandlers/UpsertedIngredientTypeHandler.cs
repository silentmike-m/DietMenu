namespace SilentMike.DietMenu.Core.Infrastructure.IngredientTypes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Events;

internal sealed class UpsertedIngredientTypeHandler : INotificationHandler<UpsertedIngredientType>
{
    private readonly ILogger<UpsertedIngredientTypeHandler> logger;

    public UpsertedIngredientTypeHandler(ILogger<UpsertedIngredientTypeHandler> logger)
        => this.logger = logger;

    public async Task Handle(UpsertedIngredientType notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("IngredientTypeId", notification.Id)
        );

        this.logger.LogInformation("Upserted ingredient type");

        await Task.CompletedTask;
    }
}
