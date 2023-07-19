namespace SilentMike.DietMenu.Mailing.Application.Identity.CommandHandlers;

using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;

internal sealed class SendResetPasswordEmailHandler : IRequestHandler<SendResetPasswordEmail>
{
    private const string EMAIL_SUBJECT = "Reset Your DietMenu account password";
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.ResetPasswordHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.ResetPasswordPlainTextEmail.xslt";

    private readonly EmailFactory emailFactory;
    private readonly ILogger<SendResetPasswordEmailHandler> logger;
    private readonly ISender mediator;

    public SendResetPasswordEmailHandler(EmailFactory emailFactory, ILogger<SendResetPasswordEmailHandler> logger, ISender mediator)
    {
        this.emailFactory = emailFactory;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(SendResetPasswordEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );

        this.logger.LogInformation("Try to prepare reset password email");

        var serializer = new XmlSerializer(typeof(SendResetPasswordEmail));
        var requestXml = serializer.Serialize(request);

        var email = await this.emailFactory.CreateEmailAsync(request.Email, requestXml, EMAIL_SUBJECT, XSLT_HTML_RESOURCE_NAME, XSLT_PLAIN_TEXT_RESOURCE_NAME, cancellationToken);

        var command = new SendEmail
        {
            Email = email,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
