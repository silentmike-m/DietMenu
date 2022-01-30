namespace SilentMike.DietMenu.Mailing.UnitTests.Users;

using System;
using System.Threading;
using System.Xml.Serialization;
using FluentAssertions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Services;
using SilentMike.DietMenu.Mailing.Application.Users.CommandHandlers;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;

[TestClass]
public sealed class SendVerifyUserEmailHandlerTests
{
    [TestMethod]
    public void ShouldThrowExceptionOnInvalidHtmlTransform()
    {
        //GIVEN
        const string plainTextResourceName = "SilentMike.DietMenu.Mailing.Application.Resources.Users.VerifyUserPlainTextEmail.xslt";

        var command = new SendVerifyUserEmail
        {
            Email = "test_email",
            Url = "url",
        };
        var serializer = new XmlSerializer(typeof(SendVerifyUserEmail));
        var commandXml = serializer.Serialize(command);
        var xmlService = new XmlService();
        var htmlXsltString = xmlService.GetXsltString(plainTextResourceName);

        //WHEN
        Func<string> action = () => xmlService.TransformToHtml(commandXml, htmlXsltString);

        //THEN
        action.Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*invalid XML document*")
            ;
    }

    [TestMethod]
    public void ShouldSendProperEmail()
    {
        SendEmail? sendEmailCommand = null;

        //GIVEN
        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((command, _) => sendEmailCommand = command as SendEmail);

        var command = new SendVerifyUserEmail
        {
            Email = "test_email",
            Url = "url",
        };

        var logger = new NullLogger<SendVerifyUserEmailHandler>();

        var commandHandler = new SendVerifyUserEmailHandler(logger, mediator.Object, new XmlService());

        //WHEN
        commandHandler.Handle(command, CancellationToken.None).Wait();

        //THEN
        sendEmailCommand.Should()
            .NotBeNull()
            ;
        sendEmailCommand!.Email.Receiver.Should()
            .Be(command.Email)
            ;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(sendEmailCommand.Email.HtmlMessage);
        htmlDocument.ParseErrors.Should()
            .BeNullOrEmpty()
            ;

        var htmlBody = htmlDocument.DocumentNode.SelectSingleNode("//html/body");
        htmlBody.Should()
            .NotBeNull()
            ;

        htmlBody.InnerHtml.Should()
            .Contain(command.Url)
            ;
    }
}
