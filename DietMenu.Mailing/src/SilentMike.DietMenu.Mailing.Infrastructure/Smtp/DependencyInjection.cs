namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Interfaces;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Services;

internal static class DependencyInjection
{
    public static void AddSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection(SmtpOptions.SECTION_NAME));

        services.AddScoped<IMailService, MailService>();
    }
}
