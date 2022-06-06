namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using System;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldThrowFormatExceptionWhenInvalidPayloadType()
    {
        //GIVEN
        var message = Mock.Of<IEmailDataMessage>(dataMessage => dataMessage.PayloadType == typeof(string).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        Func<Task> action = async () => await consumer.Consume(this.context.Object);

        //THEN
        await action.Should()
                .ThrowAsync<FormatException>()
            ;
    }

    [TestMethod]
    public async Task ShouldSendResetPasswordEmailRequest()
    {
        SendResetPasswordEmail? sendResetPasswordEmailRequest = null;

        //GIVEN
        this.mediator.Setup(i => i.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => sendResetPasswordEmailRequest = request as SendResetPasswordEmail);

        var payload = new ResetPasswordEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(ResetPasswordEmailPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(i => i.Send(It.IsAny<SendResetPasswordEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        sendResetPasswordEmailRequest.Should()
            .NotBeNull()
            ;
        sendResetPasswordEmailRequest!.Email.Should()
            .Be(payload.Email)
            ;
        sendResetPasswordEmailRequest.Url.Should()
            .Be(payload.Url)
            ;
    }

    [TestMethod]
    public async Task ShouldSendVerifyUserEmailRequest()
    {
        SendVerifyUserEmail? sendVerifyUserEmail = null;

        //GIVEN
        this.mediator.Setup(i => i.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => sendVerifyUserEmail = request as SendVerifyUserEmail);

        var payload = new VerifyUserEmailPayload
        {
            Email = "test@domain.com",
            Url = "test.com",
        };

        var payloadJson = payload.ToJson();

        var message = Mock.Of<IEmailDataMessage>(dataMessage =>
            dataMessage.Payload == payloadJson
            && dataMessage.PayloadType == typeof(VerifyUserEmailPayload).FullName);

        this.context.Setup(i => i.Message)
            .Returns(message);

        var consumer = new EmailDataMessageConsumer(this.logger, this.mediator.Object);

        //WHEN
        await consumer.Consume(this.context.Object);

        //THEN
        this.mediator.Verify(i => i.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()), Times.Once);

        sendVerifyUserEmail.Should()
            .NotBeNull()
            ;
        sendVerifyUserEmail!.Email.Should()
            .Be(payload.Email)
            ;
        sendVerifyUserEmail.Url.Should()
            .Be(payload.Url)
            ;
    }
}
