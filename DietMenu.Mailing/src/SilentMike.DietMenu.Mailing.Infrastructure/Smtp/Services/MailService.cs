namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Services;

using System.Diagnostics.CodeAnalysis;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Interfaces;

[ExcludeFromCodeCoverage]
internal sealed class MailService : IMailService
{
    private readonly SmtpOptions options;

    public MailService(IOptions<SmtpOptions> options)
        => this.options = options.Value;

    public async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken = default)
    {
        using var client = new SmtpClient();
        await client.ConnectAsync(this.options.Host, this.options.Port, this.options.UseSsl, cancellationToken);
        await client.AuthenticateAsync(this.options.User, this.options.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(quit: true, cancellationToken);
    }
}
