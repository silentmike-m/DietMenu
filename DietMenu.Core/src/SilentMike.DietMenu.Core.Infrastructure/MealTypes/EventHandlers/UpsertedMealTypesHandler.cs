namespace SilentMike.DietMenu.Core.Infrastructure.MealTypes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.Events;

internal sealed class UpsertedMealTypesHandler : INotificationHandler<UpsertedMealTypes>
{
    private readonly ILogger<UpsertedMealTypesHandler> logger;

    public UpsertedMealTypesHandler(ILogger<UpsertedMealTypesHandler> logger)
        => this.logger = logger;


    public async Task Handle(UpsertedMealTypes notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("Ids", notification.Ids)
        );

        this.logger.LogInformation("Upserted meal types");

        await Task.CompletedTask;
    }
}
