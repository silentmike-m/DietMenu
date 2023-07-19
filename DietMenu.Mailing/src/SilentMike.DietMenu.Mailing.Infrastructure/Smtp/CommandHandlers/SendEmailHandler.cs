namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp.CommandHandlers;

using Microsoft.Extensions.Options;
using MimeKit;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Models;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Infrastructure.Smtp.Interfaces;

internal sealed class SendEmailHandler : IRequestHandler<SendEmail>
{
    private readonly ILogger<SendEmailHandler> logger;
    private readonly IMailService mailService;
    private readonly SmtpOptions options;

    public SendEmailHandler(ILogger<SendEmailHandler> logger, IMailService mailService, IOptions<SmtpOptions> options)
    {
        this.logger = logger;
        this.mailService = mailService;
        this.options = options.Value;
    }

    public async Task Handle(SendEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email.Receiver),
            ("Subject", request.Email.Subject)
        );

        this.logger.LogInformation("Try to send smtp email");

        cancellationToken.ThrowIfCancellationRequested();

        var fromAddress = MailboxAddress.Parse(ParserOptions.Default, this.options.From);

        using var message = CreateMessage(request.Email, fromAddress);
        await this.mailService.SendEmailAsync(message, cancellationToken);
    }

    private static MimeMessage CreateMessage(Email email, InternetAddress fromAddress)
    {
        var toAddress = MailboxAddress.Parse(ParserOptions.Default, email.Receiver);

        var message = new MimeMessage();
        message.From.Add(fromAddress);
        message.To.Add(toAddress);
        message.Subject = email.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = email.HtmlMessage,
            TextBody = email.TextMessage,
        };

        foreach (var linkedResource in email.LinkedResources)
        {
            var emailLinkedResource = bodyBuilder.LinkedResources.Add(linkedResource.FileName, linkedResource.Data);
            emailLinkedResource.ContentId = linkedResource.ContentId;
        }

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}
