namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Email.EventHandlers;

using global::MassTransit;
using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Infrastructure.Common.Extensions;
using SilentMike.DietMenu.Core.Infrastructure.MassTransit.Email.Models;
using SilentMike.DietMenu.Shared.Email.Models;

internal sealed class ImportedFamilyDataHandler : INotificationHandler<ImportedFamilyData>
{
    private readonly IBus bus;
    private readonly ILogger<ImportedFamilyDataHandler> logger;

    public ImportedFamilyDataHandler(IBus bus, ILogger<ImportedFamilyDataHandler> logger)
    {
        this.bus = bus;
        this.logger = logger;
    }

    public async Task Handle(ImportedFamilyData notification, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", notification.FamilyId);

        this.logger.LogInformation("Try to send family data imported message");

        var results = MapResults(notification.Results);

        var payload = new ImportedFamilyDataPayload
        {
            ErrorCode = notification.ErrorCode,
            ErrorMessage = notification.ErrorMessage,
            FamilyId = notification.FamilyId,
            Results = results,
        };

        var payloadJson = payload.ToJson();

        var message = new EmailDataMessage
        {
            Payload = payloadJson,
            PayloadType = typeof(ImportedFamilyDataPayload).FullName!,
        };

        await this.bus.Publish(message, cancellationToken);
    }

    private static List<ImportFamilyDataError> MapErrors(IEnumerable<Application.Families.Models.ImportFamilyDataError> notificationErrors)
    {
        var errors = new List<ImportFamilyDataError>();

        foreach (var notificationError in notificationErrors)
        {
            var error = new ImportFamilyDataError
            {
                Code = notificationError.Code,
                Message = notificationError.Message,
            };

            errors.Add(error);
        }

        return errors;
    }

    private static List<ImportFamilyDataResult> MapResults(IEnumerable<Application.Families.Models.ImportFamilyDataResult> notificationResults)
    {
        var results = new List<ImportFamilyDataResult>();

        foreach (var notificationResult in notificationResults)
        {
            var errors = MapErrors(notificationResult.Errors);

            var result = new ImportFamilyDataResult
            {
                DataArea = notificationResult.DataArea,
                Errors = errors,
            };

            results.Add(result);
        }

        return results;
    }
}
