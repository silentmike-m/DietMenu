namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.EventHandlers;

using global::Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly IBackgroundJobClient jobClient;
    private readonly ILogger<CreatedFamilyHandler> logger;

    public CreatedFamilyHandler(IBackgroundJobClient jobClient, ILogger<CreatedFamilyHandler> logger)
    {
        this.jobClient = jobClient;
        this.logger = logger;
    }

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", notification.Id)
        );

        this.jobClient.Enqueue<ImportFamilyLibraries>(i => i.Run(notification.Id));

        await Task.CompletedTask;
    }
}
