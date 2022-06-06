namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer.EventHandlers;

using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Core.Application.Core.Events;
using SilentMike.DietMenu.Core.Infrastructure.IdentityServer4;
using SilentMike.DietMenu.Core.Infrastructure.MailingServer.Models.Core;

internal sealed class ImportedCoreDataHandler : INotificationHandler<ImportedCoreData>
{
    private const string HTTP_CLIENT_NAME = "MailingService";
    private const string HTTP_REQUEST_ENDPOINT = "Core/SendImportedCoreDataEmail";

    private readonly HttpClient httpClient;
    private readonly IdentityServer4Options identityServerOptions;
    private readonly ILogger<ImportedCoreDataHandler> logger;

    public ImportedCoreDataHandler(
        IHttpClientFactory httpClientFactory,
        IOptions<IdentityServer4Options> options,
        ILogger<ImportedCoreDataHandler> logger)
    {
        this.httpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
        this.identityServerOptions = options.Value;
        this.logger = logger;
    }

    public async Task Handle(ImportedCoreData notification, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to send imported core data message");

        var requestUri = $"{this.httpClient.BaseAddress}{HTTP_REQUEST_ENDPOINT}";

        var dataErrors = new List<ImportedCoreDataAreaErrors>();

        foreach (var notificationFamilyError in notification.Errors)
        {
            var errors = new List<ImportedCoreDataError>();

            foreach (var (errorCode, errorMessages) in notificationFamilyError.Errors)
            {
                var error = new ImportedCoreDataError
                {
                    Code = errorCode,
                    Messages = errorMessages.ToList(),
                };

                errors.Add(error);
            }

            var dataAreaError = new ImportedCoreDataAreaErrors
            {
                Errors = errors,
                DataArea = notificationFamilyError.DataArea,
            };

            dataErrors.Add(dataAreaError);
        }

        var request = new SendImportedCoreDataEmailRequest
        {
            DataErrors = dataErrors,
            IsSuccess = notification.IsSuccess,
            Server = this.identityServerOptions.Audience,
        };

        var response = await this.httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
