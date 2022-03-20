namespace SilentMike.DietMenu.Core.Infrastructure.Core.EventHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Core.Events;

internal sealed class ImportedCoreDataHandler : INotificationHandler<ImportedCoreData>
{
    private readonly ILogger<ImportedCoreDataHandler> logger;

    public ImportedCoreDataHandler(ILogger<ImportedCoreDataHandler> logger)
        => this.logger = logger;

    public async Task Handle(ImportedCoreData notification, CancellationToken cancellationToken)
    {
        var message = notification.IsSuccess
            ? "Core data have been imported"
            : "Core data import failed";

        this.logger.LogInformation("{Message}", message);

        await Task.CompletedTask;
    }
}
