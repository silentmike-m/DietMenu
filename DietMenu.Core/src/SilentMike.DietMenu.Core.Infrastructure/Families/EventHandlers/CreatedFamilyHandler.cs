namespace SilentMike.DietMenu.Core.Infrastructure.Families.EventHandlers;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Events;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly ILogger<CreatedFamilyHandler> logger;

    public CreatedFamilyHandler(ILogger<CreatedFamilyHandler> logger) => this.logger = logger;

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.Id)
        );

        this.logger.LogInformation("Family has been created");

        await Task.CompletedTask;
    }
}
