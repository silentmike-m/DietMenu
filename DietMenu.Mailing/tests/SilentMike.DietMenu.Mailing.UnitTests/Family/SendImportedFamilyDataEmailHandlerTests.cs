namespace SilentMike.DietMenu.Mailing.UnitTests.Family;

using System.Xml.Serialization;
using HtmlAgilityPack;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Family.CommandHandlers;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;
using SilentMike.DietMenu.Mailing.Application.Family.Models;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Application.Services;

[TestClass]
public sealed class SendImportedFamilyDataEmailHandlerTests
{
    private const string EMAIL_SUBJECT = "Family data imported";

    [TestMethod]
    public async Task Should_SendProper_Imported_Family_Libraries_Email()
    {
        //GIVEN
        SendEmail? sendEmailRequest = null;

        const string errorCode = "error code";
        const string dataArea = "data name";
        const string errorMessageOne = "error message 'one'";
        const string familyUserEmail = "family@domain.com";

        var emailFactory = new EmailFactory(new XmlService());
        var logger = new NullLogger<SendImportedFamilyDataEmailHandler>();
        var mediator = Substitute.For<IMediator>();

        await mediator
            .Send(Arg.Do<SendEmail>(request => sendEmailRequest = request), Arg.Any<CancellationToken>());

        mediator
            .Send(Arg.Any<GetFamilyEmail>(), Arg.Any<CancellationToken>())
            .Returns(familyUserEmail);

        var results = new List<ImportedFamilyDataResult>
        {
            new()
            {
                DataArea = dataArea,
                Errors = new List<ImportedFamilyDataError>
                {
                    new()
                    {
                        Code = errorCode,
                        Message = errorMessageOne,
                    },
                },
            },
        };

        var request = new SendImportedFamilyDataEmail
        {
            ErrorCode = null,
            ErrorMessage = null,
            IsSuccess = false,
            FamilyId = Guid.NewGuid(),
            Results = results,
        };

        var handler = new SendImportedFamilyDataEmailHandler(emailFactory, logger, mediator);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        request.IsSuccess.Should()
            .BeFalse()
            ;

        sendEmailRequest.Should()
            .NotBeNull()
            ;

        sendEmailRequest!.Email.Subject.Should()
            .Be(EMAIL_SUBJECT)
            ;

        sendEmailRequest.Email.Receiver.Should()
            .Be(familyUserEmail)
            ;

        sendEmailRequest.Email.TextMessage.Should()
            .Contain(errorCode)
            ;

        sendEmailRequest.Email.TextMessage.Should()
            .Contain(errorMessageOne)
            ;

        sendEmailRequest.Email.TextMessage.Should()
            .Contain(dataArea)
            ;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(sendEmailRequest.Email.HtmlMessage);

        htmlDocument.ParseErrors.Should()
            .BeNullOrEmpty()
            ;

        htmlDocument.DocumentNode.InnerHtml.Should()
            .Contain(errorCode)
            ;

        htmlDocument.DocumentNode.InnerHtml.Should()
            .Contain(errorMessageOne)
            ;

        htmlDocument.DocumentNode.InnerHtml.Should()
            .Contain(dataArea)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Exception_On_Invalid_Htm_Transform()
    {
        //GIVEN
        const string resourceName = "SilentMike.DietMenu.Mailing.Application.Resources.Family.ImportedFamilyDataTextEmail.xslt";

        var results = new List<ImportedFamilyDataResult>
        {
            new()
            {
                DataArea = "dataArea",
                Errors = new List<ImportedFamilyDataError>
                {
                    new()
                    {
                        Code = "errorCode",
                        Message = "errorMessageOne",
                    },
                },
            },
        };

        var request = new SendImportedFamilyDataEmail
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = Guid.NewGuid(),
            Results = results,
        };

        var serializer = new XmlSerializer(typeof(SendImportedFamilyDataEmail));
        var commandXml = serializer.Serialize(request);
        var xmlService = new XmlService();
        var htmlXsltString = await xmlService.GetXsltStringAsync(resourceName);

        //WHEN
        var action = () => xmlService.TransformToHtml(commandXml, htmlXsltString);

        //THEN
        action.Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*invalid XML document*")
            ;
    }
}
