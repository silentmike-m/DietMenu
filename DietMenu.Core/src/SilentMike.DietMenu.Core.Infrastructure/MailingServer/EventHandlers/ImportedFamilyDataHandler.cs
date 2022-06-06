namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.EventHandlers;

using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;
using SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Family;

internal sealed class ImportedFamilyDataHandler : INotificationHandler<ImportedFamilyData>
{
    private const string HTTP_CLIENT_NAME = "MailingService";
    private const string HTTP_REQUEST_ENDPOINT = "Family/SendImportedFamilyDataEmail";

    private readonly HttpClient httpClient;
    private readonly IdentityServer4Options identityServerOptions;
    private readonly ILogger<ImportedFamilyDataHandler> logger;

    public ImportedFamilyDataHandler(
        IHttpClientFactory httpClientFactory,
        IOptions<IdentityServer4Options> options,
        ILogger<ImportedFamilyDataHandler> logger)
    {
        this.httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
        this.identityServerOptions = options.Value;
        this.logger = logger;
    }

    public async Task Handle(ImportedFamilyData notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to send imported family data message");

        var requestUri = $"{this.httpClient.BaseAddress}{HTTP_REQUEST_ENDPOINT}";

        var dataErrors = new List<ImportedFamilyDataAreaErrors>();

        foreach (var notificationFamilyError in notification.Errors)
        {
            var errors = new List<ImportedFamilyDataError>();

            foreach (var (errorCode, errorMessages) in notificationFamilyError.Errors)
            {
                var error = new ImportedFamilyDataError
                {
                    Code = errorCode,
                    Messages = errorMessages.ToList(),
                };

                errors.Add(error);
            }

            var dataAreaError = new ImportedFamilyDataAreaErrors
            {
                Errors = errors,
                DataArea = notificationFamilyError.DataArea,
            };

            dataErrors.Add(dataAreaError);
        }

        var request = new SendImportedFamilyDataEmailRequest
        {
            DataErrors = dataErrors,
            FamilyId = notification.FamilyId,
            IsSuccess = notification.IsSuccess,
            Server = this.identityServerOptions.Audience,
        };

        var response = await this.httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
