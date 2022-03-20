namespace SilentMike.DietMenu.Mailing.Infrastructure.Smtp.CommandHandlers;

using MailKit.Net.Smtp;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Extensions;

internal sealed class SendEmailHandler : IRequestHandler<SendEmail>
{
    private readonly ILogger<SendEmailHandler> logger;
    private readonly SmtpOptions options;

    public SendEmailHandler(ILogger<SendEmailHandler> logger, IOptions<SmtpOptions> options)
    {
        this.logger = logger;
        this.options = options.Value;
    }

    public async Task<Unit> Handle(SendEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email.Receiver),
            ("Subject", request.Email.Subject)
        );

        this.logger.LogInformation("Try to send smtp email");

        var fromAddress = MailboxAddress.Parse(ParserOptions.Default, options.From);
        var toAddress = MailboxAddress.Parse(ParserOptions.Default, request.Email.Receiver);

        var message = new MimeMessage();
        message.From.Add(fromAddress);
        message.To.Add(toAddress);
        message.Subject = request.Email.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = request.Email.HtmlMessage,
            TextBody = request.Email.TextMessage,
        };

        foreach (var linkedResource in request.Email.LinkedResources)
        {
            var emailLinkedResource = bodyBuilder.LinkedResources.Add(linkedResource.FileName, linkedResource.Data);
            emailLinkedResource.ContentId = linkedResource.ContentId;
        }

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(options.Host, options.Port, options.UseSsl, cancellationToken);
        await client.AuthenticateAsync(options.User, options.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
