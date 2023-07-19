namespace SilentMike.DietMenu.Mailing.Application.Family.CommandHandlers;

using System.Xml.Serialization;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;

internal sealed class SendImportedFamilyDataEmailHandler : IRequestHandler<SendImportedFamilyDataEmail>
{
    private const string XSLT_HTML_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataHtmlEmail.xslt";
    private const string XSLT_PLAIN_TEXT_RESOURCE_NAME = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataTextEmail.xslt";

    private readonly EmailFactory emailFactory;
    private readonly ILogger<SendImportedFamilyDataEmailHandler> logger;
    private readonly ISender mediator;

    public SendImportedFamilyDataEmailHandler(EmailFactory emailFactory, ILogger<SendImportedFamilyDataEmailHandler> logger, ISender mediator)
    {
        this.emailFactory = emailFactory;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Handle(SendImportedFamilyDataEmail request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to prepare family data imported email");

        var getFamilyUserEmail = new GetFamilyUserEmail
        {
            FamilyId = request.FamilyId,
        };

        var familyUserEmail = await this.mediator.Send(getFamilyUserEmail, cancellationToken);

        var serializer = new XmlSerializer(typeof(SendImportedFamilyDataEmail));
        var requestXml = serializer.Serialize(request);

        var email = await this.emailFactory.CreateEmailAsync(familyUserEmail, requestXml, EmailSubjects.IMPORTED_FAMILY_DATA_EMAIL_SUBJECT, XSLT_HTML_RESOURCE_NAME, XSLT_PLAIN_TEXT_RESOURCE_NAME, cancellationToken);

        var command = new SendEmail
        {
            Email = email,
        };

        await this.mediator.Send(command, cancellationToken);
    }
}
