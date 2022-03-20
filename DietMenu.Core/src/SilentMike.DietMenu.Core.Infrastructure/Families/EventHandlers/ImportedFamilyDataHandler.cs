namespace SilentMike.DietMenu.Core.Infrastructure.Families.EventHandlers;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Families.Events;

internal sealed class ImportedFamilyDataHandler : INotificationHandler<ImportedFamilyData>
{
    private readonly ILogger<ImportedFamilyDataHandler> logger;

    public ImportedFamilyDataHandler(ILogger<ImportedFamilyDataHandler> logger)
        => this.logger = logger;

    public async Task Handle(ImportedFamilyData notification, CancellationToken cancellationToken)
    {
        var message = notification.IsSuccess
            ? "Portfolio data have been imported"
            : "Portfolio data import failed";

        this.logger.LogInformation("{Message}", message);

        await Task.CompletedTask;
    }
}
