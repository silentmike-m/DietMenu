namespace SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4;

using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Interfaces;
using SilentMike.DietMenu.Proxy.Infrastructure.IdentityServer4.Services;

internal static class DependencyInjection
{
    private const string HTTP_CLIENT_NAME = "IdentityServerService";
    private const int TIMEOUT_IN_SECONDS = 30;

    public static void AddIdentityServer4(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServer4Options>(configuration.GetSection(IdentityServer4Options.SECTION_NAME));

        var identityServerOptions = configuration.GetSection(IdentityServer4Options.SECTION_NAME).Get<IdentityServer4Options>();
        identityServerOptions ??= new IdentityServer4Options();

        services.AddHttpClient(
            HTTP_CLIENT_NAME, client =>
            {
                client.BaseAddress = new Uri(identityServerOptions.Authority);
                client.Timeout = TimeSpan.FromSeconds(TIMEOUT_IN_SECONDS);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        ).ConfigureHttpMessageHandlerBuilder(
            builder =>
            {
#if DEBUG
                builder.PrimaryHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
                };
#endif
            }
        );

        services.AddMemoryCache();
        services.AddSingleton<IIdentityServerService, IdentityServerService>();
    }
}
