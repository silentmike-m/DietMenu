namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.Extensions;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.Email.Interfaces;
using SilentMike.DietMenu.Shared.Email.Models;

[TestClass]
public sealed class EmailDataMessageConsumerTests
{
    private readonly Mock<ConsumeContext<IEmailDataMessage>> context = new();
    private readonly NullLogger<EmailDataMessageConsumer> logger = new();
    private readonly Mock<ISender> mediator = new();

    [TestMethod]
    public async Task Should_Send_Reset_Password_Email_Request()
    {
        SendResetPasswordEmail? sendResetPasswordEmailRequest = null;

        //GIVEN
        this.mediator
            .Setup(service => service.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((request, _) => sendResetPasswordEmailRequest = request as SendResetPasswordEmail);

        var payload = new ResetPasswordEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(ResetPasswordEmailPayload).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SendResetPasswordEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        sendResetPasswordEmailRequest.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }

    [TestMethod]
    public async Task Should_Send_Verify_User_Email_Request()
    {
        SendVerifyUserEmail? sendVerifyUserEmail = null;

        //GIVEN
        this.mediator
            .Setup(service => service.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest, CancellationToken>((request, _) => sendVerifyUserEmail = request as SendVerifyUserEmail);

        var payload = new VerifyUserEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(VerifyUserEmailPayload).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedRequest = new SendVerifyUserEmail
        {
            Email = payload.Email,
            Url = payload.Url,
        };

        sendVerifyUserEmail.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedRequest)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Format_Exception_When_Invalid_Payload_Type()
    {
        //GIVEN
        var message = Mock.Of<IEmailDataMessage>(dataMessage => dataMessage.PayloadType == typeof(string).FullName);

        this.context
            .Setup(consumeContext => consumeContext.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        var action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }
}
