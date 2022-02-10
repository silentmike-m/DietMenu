namespace SilentMike.DietMenu.Core.Infrastructure.MealTypes.EventHandlers;

using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.Events;

internal sealed class UpsertedMealTypeHandler : INotificationHandler<UpsertedMealType>
{
    private readonly ILogger<UpsertedMealTypeHandler> logger;

    public UpsertedMealTypeHandler(ILogger<UpsertedMealTypeHandler> logger)
        => this.logger = logger;


    public async Task Handle(UpsertedMealType notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.FamilyId),
            ("UserId", notification.UserId),
            ("MealTypeId", notification.Id)
        );

        this.logger.LogInformation("Upserted meal type");

        await Task.CompletedTask;
    }
}
