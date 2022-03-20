namespace SilentMike.DietMenu.Core.Infrastructure.MailingServer;

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

[ExcludeFromCodeCoverage]
internal static class DependencyInjection
{
    private const string HTTP_CLIENT_NAME = "MailingService";

    public static void AddMailingServer(this IServiceCollection services, IConfiguration configuration)
    {
        var mailingServerOptions = configuration.GetSection(MailingServerOptions.SectionName).Get<MailingServerOptions>();

        services.AddHttpClient(HTTP_CLIENT_NAME, client =>
        {
            client.BaseAddress = mailingServerOptions.BaseAddress;
            client.Timeout = TimeSpan.FromMilliseconds(mailingServerOptions.TimeoutInMilliSeconds);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigureHttpMessageHandlerBuilder(builder =>
        {
#if DEBUG
            builder.PrimaryHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            };
#endif
        }).AddPolicyHandler(_ =>
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(result => result.StatusCode != HttpStatusCode.OK)
                .WaitAndRetryAsync(
                    mailingServerOptions.RetryCount,
                    retryAttempt => TimeSpan.FromMilliseconds(mailingServerOptions.RetrySleepDurationInMilliSeconds)
                );
        });
    }
}
