namespace SilentMike.DietMenu.Mailing.UnitTests.Core;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Core.CommandHandlers;
using SilentMike.DietMenu.Mailing.Application.Core.Commands;
using SilentMike.DietMenu.Mailing.Application.Core.ValueModels;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Identity.Queries;
using SilentMike.DietMenu.Mailing.Application.Services;

[TestClass]
public sealed class SendImportedCoreDataEmailHandlerTests
{
    private const string EMAIL_SUBJECT = "Core data imported";

    [TestMethod]
    public void ShouldThrowExceptionOnInvalidHtmlTransform()
    {
        //GIVEN
        const string resourceName = "SilentMike.DietMenu.Mailing.Application.Resources.Core.ImportedCoreDataTextEmail.xslt";

        var command = new SendImportedCoreDataEmail();

        var serializer = new XmlSerializer(typeof(SendImportedCoreDataEmail));
        var commandXml = serializer.Serialize(command);
        var xmlService = new XmlService();
        var htmlXsltString = xmlService.GetXsltString(resourceName);

        //WHEN
        var action = () => xmlService.TransformToHtml(commandXml, htmlXsltString);

        //THEN
        action.Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*invalid XML document*")
            ;
    }

    [TestMethod]
    public void ShouldSendProperImportedCoreLibrariesEmail()
    {
        SendEmail? sendEmailCommand = null;

        //GIVEN
        const string errorCode = "error code";
        const string dataArea = "data name";
        const string errorMessageOne = "error message 'one'";
        const string errorMessageTwo = "error message 'two'";
        const string serverName = "domain.com";
        const string systemUserEmail = "system@domain.com";

        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((command, _) => sendEmailCommand = command as SendEmail);

        mediator.Setup(i => i.Send(It.IsAny<GetSystemUserEmail>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(systemUserEmail));

        var logger = new NullLogger<SendImportedCoreDataEmailHandler>();

        var command = new SendImportedCoreDataEmail
        {
            DataErrors = new List<ImportedCoreDataAreaErrors>()
            {
                new()
                {
                    Errors = new List<ImportedCoreDataError>()
                    {
                        new()
                        {
                            Code = errorCode,
                            Messages = new List<string>
                            {
                                errorMessageOne,
                                errorMessageTwo,
                            },
                        },
                    },
                    DataArea = dataArea,
                },
            },
            IsSuccess = false,
            Server = serverName,
        };
        var commandHandler = new SendImportedCoreDataEmailHandler(logger, mediator.Object, new XmlService());

        //WHEN
        commandHandler.Handle(command, CancellationToken.None).Wait();

        //THEN
        sendEmailCommand.Should()
            .NotBeNull()
            ;
        sendEmailCommand!.Email.Subject.Should()
            .Be(EMAIL_SUBJECT)
            ;
        sendEmailCommand.Email.Receiver.Should()
            .Be(systemUserEmail)
            ;
        sendEmailCommand.Email.TextMessage.Should()
            .Contain(errorCode)
            ;
        sendEmailCommand.Email.TextMessage.Should()
            .Contain(errorMessageOne)
            ;
        sendEmailCommand.Email.TextMessage.Should()
            .Contain(errorMessageTwo)
            ;
        sendEmailCommand.Email.TextMessage.Should()
            .Contain(dataArea)
            ;
        sendEmailCommand.Email.TextMessage.Should()
            .Contain(serverName)
            ;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(sendEmailCommand.Email.HtmlMessage);
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
            .Contain(errorMessageTwo)
            ;
        htmlDocument.DocumentNode.InnerHtml.Should()
            .Contain(dataArea)
            ;
        htmlDocument.DocumentNode.InnerHtml.Should()
            .Contain(serverName)
            ;
    }
}
