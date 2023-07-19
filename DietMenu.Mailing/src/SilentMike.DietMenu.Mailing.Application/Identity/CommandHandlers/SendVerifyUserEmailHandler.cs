namespace SilentMike.DietMenu.Mailing.Application.Identity.CommandHandlers;

using System.Xml.Serialization;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;

internal sealed class SendVerifyUserEmailHandler : IRequestHandler<SendVerifyUserEmail>
{
    private const string EMAIL_SUBJECT = "Verify Your DietMenu account";
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.VerifyUserHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.VerifyUserPlainTextEmail.xslt";

    private readonly EmailFactory emailFactory;
    private readonly ILogger<SendVerifyUserEmailHandler> logger;
    private readonly ISender mediator;

    public SendVerifyUserEmailHandler(EmailFactory emailFactory, ILogger<SendVerifyUserEmailHandler> logger, ISender mediator)
    {
        this.emailFactory = emailFactory;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(SendVerifyUserEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("Email", request.Email)
        );

        this.logger.LogInformation("Try to prepare verify user email");

        var serializer = new XmlSerializer(typeof(SendVerifyUserEmail));
        var requestXml = serializer.Serialize(request);

        var email = await this.emailFactory.CreateEmailAsync(request.Email, requestXml, EMAIL_SUBJECT, XSLT_HTML_RESOURCE_NAME, XSLT_PLAIN_TEXT_RESOURCE_NAME, cancellationToken);

        var command = new SendEmail
        {
            Email = email,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
