namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.EventHandlers;

using global::Hangfire;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

internal sealed class CreatedFamilyHandler : INotificationHandler<CreatedFamily>
{
    private readonly IBackgroundJobClient jobClient;

    public CreatedFamilyHandler(IBackgroundJobClient jobClient)
        => this.jobClient = jobClient;

    public async Task Handle(CreatedFamily notification, CancellationToken cancellationToken)
    {
        this.jobClient.Enqueue<ImportFamilyData>(i => i.Run(notification.Id));

        await Task.CompletedTask;
    }
}
