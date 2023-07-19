namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Interfaces;

using MimeKit;

internal interface IMailService
{
    Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken = default);
}
