﻿namespace SilentMike.DietMenu.Mailing.UnitTests.MassTransit;

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using global::MassTransit;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Consumers;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

[TestClass]
public sealed class SendVerifyUserMessageRequestConsumerTests
{
    private const string EMAIL = "user@domain.com";
    private const string URL = "login_url";

    [TestMethod]
    public async Task ShouldSendCreatedUserEmail()
    {
        SendVerifyUserEmail? command = null;

        //GIVEN
        var logger = new NullLogger<SendVerifyUserMessageRequestConsumer>();

        var mediator = new Mock<IMediator>();
        mediator.Setup(i => i.Send(It.IsAny<SendVerifyUserEmail>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<Unit>, CancellationToken>((request, _) => command = request as SendVerifyUserEmail);

        var message = new Mock<ISendVerifyUserMessageRequest>();
        message.Setup(i => i.Email)
            .Returns(EMAIL)
            ;
        message.Setup(i => i.Url)
            .Returns(URL)
            ;

        var context = new Mock<ConsumeContext<ISendVerifyUserMessageRequest>>();
        context.Setup(i => i.Message)
            .Returns(message.Object);

        var consumer = new SendVerifyUserMessageRequestConsumer(logger, mediator.Object);

        //WHEN
        await consumer.Consume(context.Object);

        //THEN
        command.Should()
            .NotBeNull()
            ;
        command!.Email.Should()
            .Be(EMAIL)
            ;
        command.Url.Should()
            .Be(URL)
            ;
    }
}