namespace SilentMike.DietMenu.Mailing.UnitTests.Identity;

using System.Xml.Serialization;
using FluentAssertions;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Emails.Commands;
using SilentMike.DietMenu.Mailing.Application.Emails.Services;
using SilentMike.DietMenu.Mailing.Application.Extensions;
using SilentMike.DietMenu.Mailing.Application.Identity.CommandHandlers;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Application.Services;

[TestClass]
public sealed class SendVerifyUserEmailHandlerTests
{
    [TestMethod]
    public async Task Should_Send_Proper_Email()
    {
        SendEmail? sendEmailCommand = null;

        //GIVEN
        var emailFactory = new EmailFactory(new XmlService());
        var logger = new NullLogger<SendVerifyUserEmailHandler>();
        var mediator = new Mock<IMediator>();

        mediator
            .Setup(service => service.Send(It.IsAny<SendEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((command, _) => sendEmailCommand = command as SendEmail);

        var request = new SendVerifyUserEmail
        {
            Email = "test_email",
            Url = "url",
        };

        var handler = new SendVerifyUserEmailHandler(emailFactory, logger, mediator.Object);

        //WHEN
        await handler.Handle(request, CancellationToken.None);

        //THEN
        sendEmailCommand.Should()
            .NotBeNull()
            ;

        sendEmailCommand!.Email.Receiver.Should()
            .Be(request.Email)
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
            .Contain(request.Url)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Exception_On_Invalid_Html_Transform()
    {
        //GIVEN
        const string plainTextResourceName = "SilentMike.DietMenu.Mailing.Application.Resources.Identity.VerifyUserPlainTextEmail.xslt";

        var command = new SendVerifyUserEmail
        {
            Email = "test_email",
            Url = "url",
        };

        var serializer = new XmlSerializer(typeof(SendVerifyUserEmail));
        var commandXml = serializer.Serialize(command);
        var xmlService = new XmlService();
        var htmlXsltString = await xmlService.GetXsltStringAsync(plainTextResourceName);

        //WHEN
        var action = () => xmlService.TransformToHtml(commandXml, htmlXsltString);

        //THEN
        action.Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*invalid XML document*")
            ;
    }
}
